using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(iInteractable))]
[CanEditMultipleObjects]
public class iInteractableEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("test")) {
        }
    }
}
