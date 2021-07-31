using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SceneToolingWindow : EditorWindow {
    [MenuItem("WAEM/Scene tooling")]
    public static void ShowWindow() {
        GetWindow<SceneToolingWindow>("Scene tooling");
    }

    //GameObject go;
    private float maxScale = 0.1f;
    private string areaText;
    private Vector2 scrollPos;
    private void OnGUI() {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Gather lightmap objects larger than:");
        //go = EditorGUILayout.ObjectField(go, typeof(GameObject), true) as GameObject;
        maxScale = EditorGUILayout.Slider(maxScale, 0f, 0.9f);
        if (GUILayout.Button("Gather", GUILayout.Height(30f))) {
            FindLargestLightmapObjects(maxScale);

        } //To replace all empty materials in the scene

        GUILayout.Space(6);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));
        EditorGUILayout.TextArea(areaText, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
        //public Material mat;
        //mat = EditorGUILayout.ObjectField(mat, typeof(Material), false) as Material; ;
        //
        //if (GUILayout.Button("A")) {
        //    MeshRenderer[] objs = GameObject.FindObjectsOfType<MeshRenderer>();
        //    foreach (var obj in objs) {
        //        bool nu = false;
        //        for (int i = 0; i < obj.sharedMaterials.Length; i++) {
        //            if (obj.sharedMaterials[i] == null) {
        //                nu = true;
        //                continue;
        //            }
        //
        //        }
        //
        //        if (nu) {
        //            Debug.Log("Null");
        //            Material[] mats = new Material[obj.sharedMaterials.Length];
        //            for (int i = 0; i < obj.sharedMaterials.Length; i++) mats[i] = mat;
        //            obj.sharedMaterials = mats;
        //        }
        //    }
        //}
    }

    private class LightmapObjectInfo {
        public string name;
        public float totalSize;
        public float sizeX;
        public float sizeY;

        public LightmapObjectInfo(string name, float sizeX, float sizeY) {
            this.name = name;
            this.totalSize = sizeX * sizeY;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }
    }

    private class MeshInfo {
        public Mesh mesh;
        public float totalSize;

        public MeshInfo(Mesh mesh) {
            this.mesh = mesh;
            this.totalSize = 0;
        }
    }

    private void FindLargestLightmapObjects(float maxScale) {
        GameObject[] objs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<string> invalidObjects = new List<string>();
        List<LightmapObjectInfo> lightmapObjectInfoList = new List<LightmapObjectInfo>();
        Dictionary<Mesh, MeshInfo> meshInfoDict = new Dictionary<Mesh, MeshInfo>();
        HashSet<MeshRenderer> toSkip = new HashSet<MeshRenderer>();
        foreach (var obj in objs) {
            if (!PrefabUtility.IsAnyPrefabInstanceRoot(obj)) continue;
            var overrides = PrefabUtility.GetObjectOverrides(obj);
            foreach (var or in overrides) {
                if (or.instanceObject.GetType() == typeof(MeshRenderer)) {
                    MeshRenderer mr = or.instanceObject as MeshRenderer;
                    if (toSkip.Contains(mr)) continue;
                    toSkip.Add(mr);
                    if (mr.lightmapIndex == -1) {
                        invalidObjects.Add(mr.name);
                        continue;
                    }
                    var so = new SerializedObject(mr);
                    var x = so.FindProperty("m_LightmapTilingOffset.x");
                    var y = so.FindProperty("m_LightmapTilingOffset.y");

                    if (float.IsNaN(x.floatValue) || float.IsNaN(y.floatValue)) {
                        invalidObjects.Add(mr.name);
                        continue;
                    }

                    if (x.floatValue > maxScale || y.floatValue > maxScale) {
                        lightmapObjectInfoList.Add(new LightmapObjectInfo(mr.name, x.floatValue, y.floatValue));
                    }

                    Mesh mesh = mr.GetComponent<MeshFilter>().sharedMesh;
                    if (meshInfoDict.TryGetValue(mesh, out MeshInfo meshInfo)) {
                        meshInfo.totalSize += x.floatValue * y.floatValue;
                    } else meshInfoDict.Add(mesh, new MeshInfo(mesh));

                    //MeshRenderer mrOrig = or.GetAssetObject() as MeshRenderer;
                    //if (mrOrig.scaleInLightmap != mr.scaleInLightmap && mrOrig.scaleInLightmap != 1f) {
                    //    //Debug.Log(mr.name + " " + mr.scaleInLightmap + "+" + mrOrig.scaleInLightmap);
                    //    or.Revert();
                    //}
                }
            }
        }

        areaText = "";
        if (lightmapObjectInfoList.Count == 0 && invalidObjects.Count == 0) areaText = "No lightmap objects found...";
        else {
            if (lightmapObjectInfoList.Count > 0) {
                areaText += $"Objects larger than {maxScale} sorted by size:\n";
                List<LightmapObjectInfo> items = new List<LightmapObjectInfo>();
                lightmapObjectInfoList.Sort((x, y) => x.totalSize > y.totalSize ? -1 : 1);
                foreach (var item in lightmapObjectInfoList)
                    areaText += $"{item.name} | {item.sizeX}, {item.sizeY}\n";
            }

            List<MeshInfo> meshInfoList = new List<MeshInfo>();
            meshInfoList.AddRange(meshInfoDict.Values);
            meshInfoList.RemoveAll(x => x.totalSize < maxScale);
            meshInfoList.Sort((x, y) => x.totalSize > y.totalSize ? -1 : 1);
            if (meshInfoList.Count > 0) {
                if (areaText.Length != 0) areaText += "\n";
                areaText += "Largest total lightmap coverage per mesh:\n";
                foreach (var meshInfo in meshInfoList) areaText += $"{meshInfo.mesh.name} | {meshInfo.totalSize}\n";
            }

            if (invalidObjects.Count > 0) {
                if (areaText.Length != 0) areaText += "\n";
                areaText += "Invalid lightmap objects:\n";
                foreach (var invalidObject in invalidObjects) areaText += invalidObject + "\n";
            }
        }
    }
}
