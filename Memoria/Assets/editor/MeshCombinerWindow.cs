using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MeshCombinerWindow : EditorWindow {
    [MenuItem("WAEM/Physics tooling")]
    public static void ShowWindow() {
        GetWindow<MeshCombinerWindow>("WAEM tooling");
    }

    private void OnGUI() {
        if (GUILayout.Button("Combine selection"))
        {
            CombineSelection();
        }
    }

    private void CombineSelection()
    {
    }
}
