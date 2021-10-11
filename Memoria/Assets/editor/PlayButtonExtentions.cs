using System.Collections;
using System.Collections.Generic;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class PlayButtonExtentions : MonoBehaviour {
    static PlayButtonExtentions() {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI() {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Play without cutscene", "Play without intro cutscene"))) {
            WijkOpeningCutscene woc = FindObjectOfType<WijkOpeningCutscene>();
            if (woc != null) {
                woc.SetEnabled(false);
                EditorUtility.SetDirty(woc);
            }
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
        if (GUILayout.Button(new GUIContent("Play with cutscene", "Play with intro cutscene"))) {
            WijkOpeningCutscene woc = FindObjectOfType<WijkOpeningCutscene>();
            if (woc != null) {
                woc.SetEnabled(true);
                EditorUtility.SetDirty(woc);
            }
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
    }
}

