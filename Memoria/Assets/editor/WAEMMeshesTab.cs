using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class WAEMMeshesTab : WAEMTab
{
    public override void Initialize()
    {
        
    }

    public override void OnGUI(GUIStyle style)
    {
        EditorGUI.indentLevel++;

        EditorGUILayout.HelpBox(
            "<size=11>Use this tool to:\n" +
            "<b>� Combine meshes</b>\tSelect the parent object of a group of objects\n" +
            "<b>� Separate submeshes</b>\tSelect the object to separate\n" +
            "<b>� Calculate submeshes</b>\tSelect a mesh or an object with a mesh\n" +
            "</size>", MessageType.Info, true);
        EditorGUI.indentLevel--;
        var layout = GUILayout.Height(40);
        if (GUILayout.Button("Combine meshes", layout)) CombineSelectionMultiMat();
        if (GUILayout.Button("Separate submeshes", layout)) ExtractSelection();
        if (GUILayout.Button("Calculate lightmap UVs for mesh", layout)) CalculateLightmapUVsForSelection();
    }

    public override void OnUpdate()
    {
        
    }

    private void CombineSelectionMultiMat() {
        if (Selection.objects.Length > 1) {
            Debug.LogError("Too many objects selected.");
            return;
        }
        if (Selection.activeGameObject == null) {
            Debug.LogError("No GameObject selected.");
            return;
        }

        var parentOfObjectsToCombine = Selection.activeGameObject;
        Undo.RegisterCompleteObjectUndo(parentOfObjectsToCombine, "combine");

        Vector3 originalPosition = parentOfObjectsToCombine.transform.position;
        parentOfObjectsToCombine.transform.position = Vector3.zero;

        MeshFilter[] filters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] renderers = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();

        Dictionary<MeshFilter, MeshRenderer> filterRendererPairs = new Dictionary<MeshFilter, MeshRenderer>();
        foreach (MeshFilter filter in filters) {
            foreach (var renderer in renderers) {
                if (filter.gameObject == renderer.gameObject) filterRendererPairs.Add(filter, renderer);
            }
        }

        List<Material> materials = new List<Material>();

        foreach (MeshRenderer renderer in renderers) {
            Material[] localMaterials = renderer.sharedMaterials;
            foreach (Material localMaterial in localMaterials) {
                if (!materials.Contains(localMaterial)) materials.Add(localMaterial);
            }
        }

        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials) {
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            foreach (MeshFilter filter in filters) {
                MeshRenderer renderer = filterRendererPairs[filter];
                if (renderer == null) {
                    Debug.LogError(filter.name + "has no MeshRenderer");
                    continue;
                }

                Material[] localMaterials = renderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++) {
                    if (localMaterials[materialIndex] != material) continue;
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = filter.transform.localToWorldMatrix;
                    combineInstances.Add(ci);
                }
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combineInstances.ToArray(), true);
            submeshes.Add(mesh);
        }

        List<CombineInstance> finalCombineInstances = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes) {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombineInstances.Add(ci);
        }

        Mesh finalMesh = new Mesh();
        try {
            finalMesh.CombineMeshes(finalCombineInstances.ToArray(), false);
        } catch (Exception ex) {
            Debug.LogError(ex.Message);
            return;
        } finally {
            parentOfObjectsToCombine.transform.position = originalPosition;
        }
        CalculateLightmapUVs(finalMesh);
        AssetDatabase.CreateAsset(finalMesh, "Assets/ContentPacks/Tooling/CombinedMeshes/" + parentOfObjectsToCombine.name + "Combined.asset");
        GameObject combinedObject = new GameObject(parentOfObjectsToCombine.name + "Combined");
        var combinedObjectFilter = combinedObject.AddComponent<MeshFilter>();
        combinedObjectFilter.sharedMesh = finalMesh;
        var combinedObjectRenderer = combinedObject.AddComponent<MeshRenderer>();
        combinedObjectRenderer.materials = materials.ToArray();
        Undo.RegisterCreatedObjectUndo(combinedObject, "combined");

        Debug.Log("Final mesh has " + submeshes.Count + " materials");
        combinedObject.isStatic = true;
        if (parentOfObjectsToCombine.transform.parent != null) {
            combinedObject.transform.parent = parentOfObjectsToCombine.transform.parent;
        }
        int index = parentOfObjectsToCombine.transform.GetSiblingIndex();
        combinedObject.transform.SetSiblingIndex(index + 1);

        parentOfObjectsToCombine.SetActive(false);
        parentOfObjectsToCombine.transform.position = originalPosition;
        combinedObject.transform.position = originalPosition;
    }

    void ExtractSelection() {
        if (Selection.objects.Length > 1) {
            Debug.LogError("Too many objects selected.");
            return;
        }
        if (Selection.activeGameObject == null) {
            Debug.LogError("No GameObject selected.");
            return;
        }

        GameObject selected = Selection.activeGameObject;
        MeshFilter selectedMeshFilter = selected.GetComponent<MeshFilter>();
        MeshRenderer selectedRenderer = selected.GetComponent<MeshRenderer>();

        if (selectedMeshFilter == null) {
            Debug.LogError("No mesh found in selected GameObject.");
            return;
        }
        if (selectedMeshFilter.sharedMesh.subMeshCount < 2)
        {
            Debug.LogError("Selected mesh filter doesn't have multiple submeshes.");
            return;
        }
        if (selectedRenderer == null) {
            Debug.LogError("No mesh renderer found in selected GameObject.");
            return;
        }

        GameObject parent = new GameObject(selected.name + "Extracted");
        Undo.RegisterCompleteObjectUndo(selected, "extract");
        Undo.RegisterCreatedObjectUndo(parent, "extractCreateObject");

        Vector3 originalPosition = selected.transform.position;
        selected.transform.position = Vector3.zero;

        if (selected.transform.parent != null) {
            parent.transform.parent = selected.transform.parent;
        }
        int index = selected.transform.GetSiblingIndex();
        parent.transform.SetSiblingIndex(index + 1);

        for (int i = 0; i < selectedMeshFilter.sharedMesh.subMeshCount; i++) {
            Mesh extracted = ExtractMesh(selectedMeshFilter.sharedMesh, i);
            AssetDatabase.CreateAsset(extracted, "Assets/ContentPacks/Tooling/ExtractedMeshes/" + selected.name + "Extracted" + i + ".asset");
            GameObject obj = new GameObject(selected.name + i);
            MeshFilter filter = obj.AddComponent<MeshFilter>();
            MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = selectedRenderer.sharedMaterials[i];
            filter.sharedMesh = extracted;
            obj.transform.parent = parent.transform;
            obj.isStatic = true;
        }

        selected.SetActive(false);
        selected.transform.position = originalPosition;
        parent.transform.position = originalPosition;
        parent.isStatic = true;
    }

    private void CalculateLightmapUVsForSelection() {
        if (Selection.objects.Length > 1) {
            Debug.LogError("Too many objects selected.");
            return;
        }

        Mesh mesh = null;
        if (Selection.activeObject.GetType() == typeof(Mesh)) {
            mesh = Selection.activeObject as Mesh;

        } else if (Selection.activeGameObject != null) {
            mesh = Selection.activeGameObject.GetComponent<MeshFilter>().sharedMesh;
            if (mesh == null) {
                Debug.LogError("No mesh found in selected GameObject.");
                return;
            }
        } else {
            Debug.LogError("No GameObject or Mesh selected.");
        }

        Undo.RecordObject(mesh, "CalculateLightmapUVs");
        if (mesh != null) {
            Debug.Log("Succesfully calculated lightmap UVs");
            CalculateLightmapUVs(mesh);
        }
    }

    private Mesh ExtractMesh(Mesh m, int meshIndex) {
        var vertices = m.vertices;
        var normals = m.normals;
        var UVs = m.uv;
        var newVerts = new List<Vector3>();
        var newNorms = new List<Vector3>();
        var newTris = new List<int>();
        var newUVs = new List<Vector2>();
        var triangles = m.GetTriangles(meshIndex);
        for (var i = 0; i < triangles.Length; i += 3) {
            var A = triangles[i + 0];
            var B = triangles[i + 1];
            var C = triangles[i + 2];
            newVerts.Add(vertices[A]);
            newVerts.Add(vertices[B]);
            newVerts.Add(vertices[C]);
            newNorms.Add(normals[A]);
            newNorms.Add(normals[B]);
            newNorms.Add(normals[C]);
            newUVs.Add(UVs[A]);
            newUVs.Add(UVs[B]);
            newUVs.Add(UVs[C]);
            newTris.Add(newTris.Count);
            newTris.Add(newTris.Count);
            newTris.Add(newTris.Count);
        }
        var mesh = new Mesh();
        mesh.indexFormat = newVerts.Count > ushort.MaxValue ? IndexFormat.UInt32 : IndexFormat.UInt16;
        mesh.SetVertices(newVerts);
        mesh.SetNormals(newNorms);
        mesh.SetTriangles(newTris, 0, true);
        mesh.SetUVs(0, newUVs);

        CalculateLightmapUVs(mesh);
        return mesh;
    }

    private void CalculateLightmapUVs(Mesh mesh) {
        UnwrapParam param;
        UnwrapParam.SetDefaults(out param);
        Unwrapping.GenerateSecondaryUVSet(mesh, param);
    }
}
