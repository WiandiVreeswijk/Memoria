using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MeshCombinerWindow : ScriptableWizard {
    private List<Light> lights = new List<Light>();

    [MenuItem("WAEM/Mesh combine tooling")]
    public static void ShowWindow() {
        var window = GetWindow<MeshCombinerWindow>("WAEM tooling");
    }

    private void OnSelectionChange() {
    }

    private void OnGUI() {
        if (GUILayout.Button("Combine", GUILayout.Height(100))) {
            CombineSelectionMultiMat(false);
        }
        if (GUILayout.Button("Combine and save as preset")) {
            CombineSelectionMultiMat(true);
        }
    }

    private void CombineSelectionMultiMat(bool createPrefab) {
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
        Unwrapping.GenerateSecondaryUVSet(finalMesh);
        AssetDatabase.CreateAsset(finalMesh, "Assets/ContentPacks/CombinedMeshes/" + parentOfObjectsToCombine.name + "Combined.asset");
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
            int index = parentOfObjectsToCombine.transform.GetSiblingIndex();
            combinedObject.transform.SetSiblingIndex(index + 1);
        }

        parentOfObjectsToCombine.SetActive(false);
        parentOfObjectsToCombine.transform.position = originalPosition;
        combinedObject.transform.position = originalPosition;
    }

    //private void CombineSelection(bool createPrefab) {
    //    if (Selection.objects.Length > 1) {
    //        Debug.LogError("Too many objects selected.");
    //        return;
    //    }
    //    if (Selection.activeGameObject == null) {
    //        Debug.LogError("No GameObject selected.");
    //        return;
    //    }
    //
    //    var parentOfObjectsToCombine = Selection.activeGameObject;
    //
    //    Vector3 originalPosition = parentOfObjectsToCombine.transform.position;
    //    parentOfObjectsToCombine.transform.position = Vector3.zero;
    //
    //    MeshFilter[] meshFilters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>(false);
    //    Dictionary<Material, List<MeshFilter>> materialToMeshFilterList = new Dictionary<Material, List<MeshFilter>>();
    //    List<GameObject> combinedObjects = new List<GameObject>();
    //
    //    for (int i = 0; i < meshFilters.Length; i++) {
    //        var materials = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterials;
    //        if (materials == null) continue;
    //        if (materials.Length > 1) {
    //            //parentOfObjectsToCombine.transform.position = originalPosition;
    //            //Debug.LogError("Multiple materials on meshes. Can't combine");
    //            //return;
    //        }
    //
    //        Material material = materials[0];
    //        if (materialToMeshFilterList.ContainsKey(material)) materialToMeshFilterList[material].Add(meshFilters[i]);
    //        else materialToMeshFilterList.Add(material, new List<MeshFilter>() { meshFilters[i] });
    //    }
    //
    //    foreach (var entry in materialToMeshFilterList) {
    //        List<MeshFilter> meshesWithSameMaterial = entry.Value;
    //        string materialName = entry.Key.ToString().Split(' ')[0];
    //        CombineInstance[] combine = new CombineInstance[meshesWithSameMaterial.Count];
    //        for (int i = 0; i < meshesWithSameMaterial.Count; i++) {
    //            combine[i].mesh = meshesWithSameMaterial[i].sharedMesh;
    //            combine[i].transform = meshesWithSameMaterial[i].transform.localToWorldMatrix;
    //        }
    //
    //        Mesh combinedMesh = new Mesh();
    //        combinedMesh.CombineMeshes(combine, true, true, true);
    //        materialName += "_" + combinedMesh.GetInstanceID();
    //        Unwrapping.GenerateSecondaryUVSet(combinedMesh);
    //        if (createPrefab) AssetDatabase.CreateAsset(combinedMesh, "Assets/ContentPacks/CombinedMeshes/" + materialName + "Combined.asset");
    //
    //        string goName = (materialToMeshFilterList.Count > 1) ? materialName + "Combined" : parentOfObjectsToCombine.name + "Combined";
    //        GameObject combinedObject = new GameObject(goName);
    //        var filter = combinedObject.AddComponent<MeshFilter>();
    //        filter.sharedMesh = combinedMesh;
    //        var renderer = combinedObject.AddComponent<MeshRenderer>();
    //        renderer.sharedMaterial = entry.Key;
    //        combinedObjects.Add(combinedObject);
    //
    //
    //    }
    //
    //    GameObject resultGO = null;
    //    if (combinedObjects.Count > 1) {
    //        resultGO = new GameObject(parentOfObjectsToCombine.name + "Combined");
    //        foreach (var combinedObject in combinedObjects) combinedObject.transform.parent = resultGO.transform;
    //    } else {
    //        resultGO = combinedObjects[0];
    //    }
    //
    //    resultGO.isStatic = true;
    //    if (parentOfObjectsToCombine.transform.parent != null) {
    //        resultGO.transform.parent = parentOfObjectsToCombine.transform.parent;
    //    }
    //
    //    if (createPrefab) PrefabUtility.SaveAsPrefabAssetAndConnect(resultGO, "Assets/ContentPacks/CombinedMeshes" + resultGO.name + ".prefab", InteractionMode.UserAction);
    //    parentOfObjectsToCombine.SetActive(false);
    //    parentOfObjectsToCombine.transform.position = originalPosition;
    //    resultGO.transform.position = originalPosition;
    //}
}
