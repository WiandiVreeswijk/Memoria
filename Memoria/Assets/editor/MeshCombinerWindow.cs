using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MeshCombinerWindow : EditorWindow {
    private List<Light> lights = new List<Light>();

    [MenuItem("WAEM/Physics tooling")]
    public static void ShowWindow() {
        var window = GetWindow<MeshCombinerWindow>("WAEM tooling");
        window.Initialize();
    }



    private void Initialize() {

    }

    private void OnDestroy() {
        lights.Clear();
    }

    private void OnSelectionChange() {
        lights.Clear();
        foreach (var obj in Selection.gameObjects) {
            Light light = obj.GetComponent<Light>();
            if (light != null) lights.Add(light);
            Debug.Log("Yes");
        }
    }

    private float fl = 1000f;
    private void OnGUI() {
        if (GUILayout.Button("Combine selection")) {
            CombineSelection();
        }

        if (lights.Count > 0)
        {
            fl = EditorGUILayout.Slider(fl, 1000f, 40000f);
            Debug.Log(Mathf.CorrelatedColorTemperatureToRGB(fl));
            EditorGUILayout.ColorField(Mathf.CorrelatedColorTemperatureToRGB(fl));
        }
        else GUILayout.Label("No lights selected");
    }

    private void CombineSelection() {
    }
}
