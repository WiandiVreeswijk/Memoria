using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class WAEMMeshesTab : IWAEMTab {
    private float packMargin = 0.04f;

    public void Initialize() {

    }

    public void OnGUI(EditorWindow window, GUIStyle style) {
        EditorGUI.indentLevel++;
        GUI.skin.FindStyle("HelpBox").richText = true;
        EditorGUILayout.HelpBox(
            "<size=11>Use this tool to:\n" +
            "<b>• Combine meshes</b>\tSelect the parent object of a group of objects\n" +
            "<b>• Separate submeshes</b>\tSelect the object to separate\n" +
            "<b>• Calculate submeshes</b>\tSelect a mesh or an object with a mesh\n" +
            "<b>• Bake colliders</b>\tSelect a mesh to generate a mesh collider\n" +
            "</size>", MessageType.Info, true);
        EditorGUI.indentLevel--;
        var layout = GUILayout.Height(40);
        if (GUILayout.Button("Combine meshes", layout)) CombineSelectionMultiMat(false);
        if (GUILayout.Button("Combine meshes and force one material", layout)) CombineSelectionMultiMat(true);
        if (GUILayout.Button("Separate submeshes", layout)) ExtractSelection();
        GUILayout.BeginHorizontal();
        packMargin = GUILayout.HorizontalSlider(packMargin, 0.004f, 0.1f);
        if (GUILayout.Button("Calculate lightmap UVs for mesh", layout)) CalculateLightmapUVsForSelection(packMargin);
        GUILayout.EndHorizontal();
        //if (GUILayout.Button("Calculate height UVs for mesh", layout)) CalculateHeightUVsForSelection(false);
        //if (GUILayout.Button("Calculate height UVs for mesh flipped", layout)) CalculateHeightUVsForSelection(true);
        if (GUILayout.Button("Fix height UVs for selected plant", layout)) FixHeightUVsForSelection(false);
        if (GUILayout.Button("Fix height UVs for selected plant flipped", layout)) FixHeightUVsForSelection(true);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Bake collider", layout)) BakeMeshCollider();
        GUILayout.EndHorizontal();

    }

    public void OnUpdate() {

    }

    public void OnSelectionChange(EditorWindow window) {

    }

    public void OnDestroy() {

    }

    private void CombineSelectionMultiMat(bool useOneMaterial) {
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
        if (!useOneMaterial) {
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

        } else {
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            foreach (MeshFilter filter in filters) {
                MeshRenderer renderer = filterRendererPairs[filter];
                if (renderer == null) {
                    Debug.LogError(filter.name + "has no MeshRenderer");
                    continue;
                }

                CombineInstance ci = new CombineInstance();
                ci.mesh = filter.sharedMesh;
                ci.subMeshIndex = 0;
                ci.transform = filter.transform.localToWorldMatrix;
                combineInstances.Add(ci);
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
        if (useOneMaterial) {
            combinedObjectRenderer.material = materials[0];
        } else {
            combinedObjectRenderer.materials = materials.ToArray();
        }
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
        if (selectedMeshFilter.sharedMesh.subMeshCount < 2) {
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

        Quaternion originalRotation = selected.transform.rotation;
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
        selected.transform.rotation = originalRotation;
        parent.transform.position = originalPosition;
        parent.transform.rotation = originalRotation;
        parent.isStatic = true;
    }

    private void BakeMeshCollider() {
        if (Selection.objects.Length > 1) {
            Debug.LogError("Too many objects selected.");
            return;
        }
        if (Selection.activeGameObject == null) {
            Debug.LogError("No GameObject selected.");
            return;
        }

        MeshFilter selectedMeshFilter = Selection.activeGameObject.GetComponent<MeshFilter>();
        if (selectedMeshFilter == null) {
            Debug.LogError("No mesh found in selected GameObject.");
            return;
        }

        Physics.BakeMesh(selectedMeshFilter.sharedMesh.GetInstanceID(), true);

        MeshCollider collider = Selection.activeGameObject.GetComponent<MeshCollider>();
        if (collider == null) {
            collider = Selection.activeGameObject.AddComponent<MeshCollider>();
        }

        collider.sharedMesh = selectedMeshFilter.sharedMesh;
    }


    private void FixHeightUVsForSelection(bool flipped) {
        //Undo.SetCurrentGroupName("CalculateHeightUVsGroup");
        if (Selection.gameObjects.Length > 0) {
            List<MeshFilter> filters = new List<MeshFilter>();
            foreach (var obj in Selection.gameObjects) {
                if (obj.GetType() == typeof(GameObject)) {
                    MeshFilter filter = ((GameObject)obj).GetComponent<MeshFilter>();
                    if (filter != null && filter.sharedMesh != null && !filters.Contains(filter)) filters.Add(filter);
                }
            }
            int found = 0;
            filters.RemoveAll(x => {
                if (x.sharedMesh.name.EndsWith("_heightUV")) return true;
                Mesh mesh = AssetDatabase.LoadAssetAtPath("Assets/ContentPacks/Tooling/VegetationMeshes/" + x.sharedMesh.name + "_heightUV.asset",
                    typeof(Mesh)) as Mesh;
                if (mesh == null) return false;
                found++;
                Undo.RecordObject(x, "CalculateHeightUVs");
                x.sharedMesh = mesh;
                return true;
            });

            int count = 0;
            Dictionary<Mesh, List<MeshFilter>> meshDict = new Dictionary<Mesh, List<MeshFilter>>();
            foreach (var filter in filters) {
                foreach (var filter2 in filters) {
                    if (filter == filter2) continue;
                    meshDict.TryGetValue(filter.sharedMesh, out List<MeshFilter> list);
                    if (list == null) {
                        list = new List<MeshFilter>();
                        meshDict.Add(filter.sharedMesh, list);
                    }

                    count++;
                    list.Add(filter);
                }
            }
            if (meshDict.Count > 0) {
                Mesh[] meshes = new Mesh[meshDict.Count];
                Vector3[][] vertices = new Vector3[meshDict.Count][];
                Vector2[][] newUVs = new Vector2[meshDict.Count][];
                float[] minVerts = new float[meshDict.Count];
                float[] maxVerts = new float[meshDict.Count];
                int i = 0;
                foreach (var pair in meshDict) {
                    meshes[i] = new Mesh();
                    meshes[i].name = pair.Key.name + "_heightUV";
                    meshes[i].vertices = pair.Key.vertices;
                    meshes[i].triangles = pair.Key.triangles;
                    meshes[i].uv = pair.Key.uv;
                    meshes[i].uv2 = pair.Key.uv2;
                    meshes[i].uv3 = pair.Key.uv3;
                    meshes[i].uv4 = pair.Key.uv4;
                    meshes[i].normals = pair.Key.normals;
                    meshes[i].colors = pair.Key.colors;
                    meshes[i].tangents = pair.Key.tangents;
                    vertices[i] = pair.Key.vertices;
                    minVerts[i] = float.MaxValue;
                    maxVerts[i] = float.MinValue;
                    i++;
                }

                Parallel.For(0, meshDict.Count, x => {
                    foreach (var vert in vertices[x]) {
                        if (vert.y < minVerts[x]) minVerts[x] = vert.y;
                        if (vert.y > maxVerts[x]) maxVerts[x] = vert.y;
                    }

                    newUVs[x] = new Vector2[vertices[x].Length];
                    for (int i = 0; i < vertices[x].Length; i++) {
                        newUVs[x][i] = new Vector2(0, Utils.Remap(vertices[x][i].y, minVerts[x], maxVerts[x], flipped ? 1 : 0, flipped ? 0 : 1));
                    }
                });
                int j = 0;
                foreach (var pair in meshDict) {
                    meshes[j].uv3 = newUVs[j];
                    Debug.Log("Created mesh asset at " + "Assets/ContentPacks/Tooling/VegetationMeshes/" + pair.Key.name + "_heightUV.asset");
                    AssetDatabase.CreateAsset(meshes[j], "Assets/ContentPacks/Tooling/VegetationMeshes/" + pair.Key.name + "_heightUV.asset");
                    foreach (var mesh in pair.Value) {
                        Undo.RecordObject(mesh, "CalculateHeightUVs");
                        mesh.sharedMesh = meshes[j];
                        Debug.Log("MeshesTab1");
                        EditorUtility.SetDirty(mesh);
                    }
                    j++;
                }

                Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                AssetDatabase.SaveAssets();
            }
            Debug.Log("Succesfully calculated height UVs for " + meshDict.Count + " new meshgroups and " + found + " existing meshes. Totalling " + count + " different meshes");
        }
        AssetDatabase.SaveAssets();
    }

    private void CalculateHeightUVsForSelection(bool flipped) {
        if (Selection.objects.Length > 1) {
            List<Mesh> meshes = new List<Mesh>();
            foreach (var obj in Selection.objects) {
                if (obj.GetType() == typeof(Mesh) && !meshes.Contains(obj as Mesh)) meshes.Add(obj as Mesh);
                else if (obj.GetType() == typeof(GameObject)) {
                    MeshFilter filter = ((GameObject)obj).GetComponent<MeshFilter>();
                    if (filter != null && filter.sharedMesh != null && !meshes.Contains(filter.sharedMesh)) meshes.Add(filter.sharedMesh);
                }
            }

            if (meshes.Count > 0) {
                Vector3[][] vertices = new Vector3[meshes.Count][];
                Vector2[][] newUVs = new Vector2[meshes.Count][];
                float[] minVerts = new float[meshes.Count];
                float[] maxVerts = new float[meshes.Count];
                for (int i = 0; i < meshes.Count; i++) {
                    vertices[i] = meshes[i].vertices;
                    minVerts[i] = float.MaxValue;
                    maxVerts[i] = float.MinValue;
                }

                Parallel.For(0, meshes.Count, x => {
                    foreach (var vert in vertices[x]) {
                        if (vert.y < minVerts[x]) minVerts[x] = vert.y;
                        if (vert.y > maxVerts[x]) maxVerts[x] = vert.y;
                    }

                    newUVs[x] = new Vector2[vertices[x].Length];
                    for (int i = 0; i < vertices[x].Length; i++) {
                        newUVs[x][i] = new Vector2(0, Utils.Remap(vertices[x][i].y, minVerts[x], maxVerts[x], flipped ? 1 : 0, flipped ? 0 : 1));
                    }
                });

                Undo.SetCurrentGroupName("CalculateHeightUVsGroup");
                for (int i = 0; i < meshes.Count; i++) {
                    Undo.RecordObject(meshes[i], "CalculateHeightUVs");
                    meshes[i].uv3 = newUVs[i];
                }

                Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                Debug.Log("Succesfully calculated height UVs for " + meshes.Count + " meshes");
            } else Debug.LogError("No mesh found in selected Objects.");
        } else {

            Mesh mesh = null;
            if (Selection.activeObject.GetType() == typeof(Mesh)) {
                mesh = Selection.activeObject as Mesh;

            } else if (Selection.activeGameObject != null) {
                mesh = Selection.activeGameObject.GetComponent<MeshFilter>()?.sharedMesh;
                if (mesh == null) {
                    Debug.LogError("No mesh found in selected GameObject.");
                    return;
                }
            } else {
                Debug.LogError("No GameObject or Mesh selected.");
            }

            if (mesh != null) {
                CalculateHeightUVs(mesh, flipped);
                Debug.Log("Succesfully calculated height UVs");
            }
        }
        AssetDatabase.SaveAssets();
    }

    private void CalculateHeightUVs(Mesh mesh, bool flipped) {
        Undo.RecordObject(mesh, "CalculateHeightUVs");
        float minVert = float.MaxValue;
        float maxVert = float.MinValue;

        foreach (var vert in mesh.vertices) {
            if (vert.y < minVert) minVert = vert.y;
            if (vert.y > maxVert) maxVert = vert.y;
        }

        Vector2[] newUVs = new Vector2[mesh.uv.Length];
        for (int i = 0; i < mesh.uv.Length; i++) {
            newUVs[i] = new Vector2(0, Utils.Remap(mesh.vertices[i].y, minVert, maxVert, flipped ? 1 : 0, flipped ? 0 : 1));
        }

        mesh.uv3 = newUVs;
    }

    private void CalculateLightmapUVsForSelection(float packMargin) {
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
            CalculateLightmapUVs(mesh, packMargin);
            Debug.Log("Succesfully calculated lightmap UVs");
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

    private void CalculateLightmapUVs(Mesh mesh, float packMargin = 0.04f) {
        UnwrapParam param;
        UnwrapParam.SetDefaults(out param);
        param.packMargin = packMargin;
        //Debug.Log(param.hardAngle);
        //Debug.Log(param.angleError);
        //Debug.Log(param.areaError);
        //Debug.Log(param.packMargin);
        Unwrapping.GenerateSecondaryUVSet(mesh, param);
    }
}
