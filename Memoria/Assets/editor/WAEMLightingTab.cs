using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WAEMLightingTab : IWAEMTab {
    private float temperature = 2000f;
    public void Initialize() {
    }


    public void OnGUI(EditorWindow window, GUIStyle style) {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null) {
            EditorGUILayout.LabelField("<color=red>No GameObject selected</color>", style);
        } else {
            if (Selection.gameObjects.Length > 1) {
                List<Light> lights = new List<Light>();
                foreach (GameObject obj in Selection.gameObjects) {
                    Light light = obj.GetComponent<Light>();
                    if (light != null) lights.Add(light);
                }
                if (lights.Count == 0) EditorGUILayout.LabelField("<color=red>No GameObjects with lights selected</color>", style);
                else OnLightSelectedGUI(window, style, lights);
            } else {
                Light light = selectedObject.GetComponent<Light>();
                if (light == null) EditorGUILayout.LabelField("<color=red>No light selected</color>", style);
                else OnLightSelectedGUI(window, style, new List<Light>() { light });
            }
        }
    }

    private void OnLightSelectedGUI(EditorWindow window, GUIStyle style, List<Light> lights) {
        EditorGUILayout.LabelField("<b>Selected</b> " + (lights.Count == 1 ? lights[0].name : (lights.Count + " lights")), style);

        EditorGUI.BeginChangeCheck();
        temperature = EditorGUILayout.Slider(temperature, 1000f, 40000f);
        if (EditorGUI.EndChangeCheck()) {
            Undo.SetCurrentGroupName("SetLightTemperatureColorGroup");
            Color color = Mathf.CorrelatedColorTemperatureToRGB(temperature);
            foreach (Light light in lights)
            {
                Undo.RecordObject(light, "SetLightTemperatureColor");
                light.color = color;
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ColorField(Mathf.CorrelatedColorTemperatureToRGB(temperature));
        EditorGUI.EndDisabledGroup();

        //GUI.BeginGroup()
    }

    public void OnUpdate() {

    }

    public void OnSelectionChange(EditorWindow window) {
        window.Repaint();
    }

    public void OnDestroy() {

    }
}
