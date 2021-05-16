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
            CombineSelection(false);
        }
        if (GUILayout.Button("Combine and save as preset")) {
            CombineSelection(true);
        }
    }

    private void CombineSelection(bool createPrefab) {
        if (Selection.objects.Length > 1) {
            Debug.LogError("Too many objects selected.");
            return;
        }
        if (Selection.activeGameObject == null) {
            Debug.LogError("No GameObject selected.");
            return;
        }

        var parentOfObjectsToCombine = Selection.activeGameObject;

        Vector3 originalPosition = parentOfObjectsToCombine.transform.position;
        parentOfObjectsToCombine.transform.position = Vector3.zero;

        MeshFilter[] meshFilters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
        Dictionary<Material, List<MeshFilter>> materialToMeshFilterList = new Dictionary<Material, List<MeshFilter>>();
        List<GameObject> combinedObjects = new List<GameObject>();

        for (int i = 0; i < meshFilters.Length; i++) {
            var materials = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterials;
            if (materials == null) continue;
            if (materials.Length > 1) {
                parentOfObjectsToCombine.transform.position = originalPosition;
                Debug.LogError("Multiple materials on meshes. Can't combine");
                return;
            }

            Material material = materials[0];
            if (materialToMeshFilterList.ContainsKey(material)) materialToMeshFilterList[material].Add(meshFilters[i]);
            else materialToMeshFilterList.Add(material, new List<MeshFilter>() { meshFilters[i] });
        }

        foreach (var entry in materialToMeshFilterList) {
            List<MeshFilter> meshesWithSameMaterial = entry.Value;
            string materialName = entry.Key.ToString().Split(' ')[0];
            CombineInstance[] combine = new CombineInstance[meshesWithSameMaterial.Count];
            for (int i = 0; i < meshesWithSameMaterial.Count; i++) {
                combine[i].mesh = meshesWithSameMaterial[i].sharedMesh;
                combine[i].transform = meshesWithSameMaterial[i].transform.localToWorldMatrix;
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine, true, true, true);
            materialName += "_" + combinedMesh.GetInstanceID();
            Unwrapping.GenerateSecondaryUVSet(combinedMesh);
            if (createPrefab) AssetDatabase.CreateAsset(combinedMesh, "Assets/ContentPacks/CombinedMeshes/" + materialName + "Combined.asset");

            string goName = (materialToMeshFilterList.Count > 1) ? materialName + "Combined" : parentOfObjectsToCombine.name + "Combined";
            GameObject combinedObject = new GameObject(goName);
            var filter = combinedObject.AddComponent<MeshFilter>();
            filter.sharedMesh = combinedMesh;
            var renderer = combinedObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = entry.Key;
            combinedObjects.Add(combinedObject);

            if (parentOfObjectsToCombine.transform.parent != null) {
                combinedObject.transform.parent = parentOfObjectsToCombine.transform.parent;
            }
            combinedObject.isStatic = true;
        }

        GameObject resultGO = null;
        if (combinedObjects.Count > 1) {
            resultGO = new GameObject("CombinedMeshes_" + parentOfObjectsToCombine.name);
            foreach (var combinedObject in combinedObjects) combinedObject.transform.parent = resultGO.transform;
        } else {
            resultGO = combinedObjects[0];
        }

        if (createPrefab) PrefabUtility.SaveAsPrefabAssetAndConnect(resultGO, "Assets/ContentPacks/CombinedMeshes" + resultGO.name + ".prefab", InteractionMode.UserAction);
        parentOfObjectsToCombine.SetActive(false);
        parentOfObjectsToCombine.transform.position = originalPosition;
        resultGO.transform.position = originalPosition;
    }
}
