using System.Collections.Generic;
using System.Linq;
using PlasticGui.Help.NewVersions;
using UnityEngine;
using UnityEditor;
public class FindMissingScriptsRecursively : EditorWindow {
    static int go_count = 0, components_count = 0, missing_count = 0;
    private float percentage = 0.5f;
    [MenuItem("WAEM/Objects")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
    }

    public void OnGUI() {
        if (GUILayout.Button("Find Missing Scripts in selected GameObjects")) {
            FindInSelected();
        }
        if (GUILayout.Button("Find not navigation static objects")) {
            FindNonStatic();
        }

        if (GUILayout.Button("Find shaders and materials")) {
            FindShaderAndMaterials();
        }
        if (GUILayout.Button("Find Lightmap sizes")) {
            FindLightmaps();
        }

        if (GUILayout.Button("Count meshes")) {
            CountMeshes();
        }


        percentage = EditorGUILayout.Slider(percentage, 0.0f, 1.0f);
        if (GUILayout.Button("Select percentage")) {
            SelectPercentage();
        }
    }

    private void SelectPercentage() {
        List<Object> objects = Selection.objects.ToList();
        objects.RemoveAll(x => UnityEngine.Random.Range(0.0f, 1.0f) > percentage);
        Selection.objects = objects.ToArray();
    }

    private void CountMeshes() {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>(true);
        Dictionary<Mesh, int> dict = new Dictionary<Mesh, int>();
        foreach (GameObject go in gameObjects) {
            MeshFilter renderer = go.GetComponent<MeshFilter>();
            if (renderer != null) {
                if (!dict.ContainsKey(renderer.sharedMesh)) {
                    dict.Add(renderer.sharedMesh, 0);
                }
                dict[renderer.sharedMesh]++;
            }
        }

        var list = dict.ToList();
        list.Sort((x, y) => x.Value > y.Value ? -1 : 1);
        string str = "";
        foreach (var obj in list) {
            str += $"{obj.Key.name}|{obj.Value}\n";
        }

        Debug.Log(str);
    }
    struct LightmapObject {
        public Vector4 offset;
        public GameObject gameObject;
    }
    private void FindLightmaps() {
        List<LightmapObject> objs = new List<LightmapObject>();
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject go in gameObjects) {
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null && renderer.lightmapIndex != -1) {
                LightmapObject obj = new LightmapObject();
                obj.gameObject = go;
                obj.offset = renderer.lightmapScaleOffset;
                objs.Add(obj);
            }
        }

        objs.Sort((x, y) => (x.offset.x * x.offset.y) > (y.offset.x * y.offset.y) ? -1 : 1);

        string str = "";
        foreach (LightmapObject lmo in objs) {
            str += $"{lmo.offset.x}|{lmo.offset.y}: {lmo.gameObject.name}\n";
        }

        Debug.Log(str);
    }

    private void FindShaderAndMaterials() {
        Dictionary<Shader, Dictionary<string, int>> dict = new Dictionary<Shader, Dictionary<string, int>>();

        string str = "";
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject go in gameObjects) {
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null) {
                foreach (Material material in renderer.sharedMaterials) {
                    if (material != null) {
                        if (!dict.ContainsKey(material.shader)) {
                            dict.Add(material.shader, new Dictionary<string, int>());
                        }

                        if (material.name == "Lit") Debug.Log(go.name);
                        if (!dict[material.shader].ContainsKey(material.name)) {
                            dict[material.shader].Add(material.name, 0);
                        }

                        dict[material.shader][material.name]++;
                    }
                }
            }
        }

        foreach (var entry in dict) {
            str += entry.Key.name + "\n";
            foreach (var mat in entry.Value) {
                str += "\t" + mat + "\n";
            }
        }
        Debug.Log(str);
    }

    private static void FindInSelected() {
        GameObject[] go = Selection.gameObjects;
        go_count = 0;
        components_count = 0;
        missing_count = 0;
        foreach (GameObject g in go) {
            FindInGO(g);
        }
        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }

    private void FindNonStatic() {
        string objectsString = "";
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in gameObjects) {
            if (go.GetComponent<MeshRenderer>() != null && !GameObjectUtility.AreStaticEditorFlagsSet(go, StaticEditorFlags.NavigationStatic)) {
                objectsString += go.name + "\n";
            }
        }
        Debug.Log(objectsString);
    }

    private static void FindInGO(GameObject g) {
        go_count++;
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++) {
            components_count++;
            if (components[i] == null) {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null) {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Debug.Log(s + " has an empty script attached in position: " + i, g);
            }
        }
        foreach (Transform childT in g.transform) {
            FindInGO(childT.gameObject);
        }
    }
}