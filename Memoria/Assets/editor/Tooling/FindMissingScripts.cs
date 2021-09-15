using UnityEngine;
using UnityEditor;
public class FindMissingScriptsRecursively : EditorWindow {
    static int go_count = 0, components_count = 0, missing_count = 0;

    [MenuItem("WAEM/Find objects")]
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