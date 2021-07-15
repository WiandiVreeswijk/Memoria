using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PortalVisual))]
[CanEditMultipleObjects]
public class PortalVisualEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        PortalVisual portalVisual = target as PortalVisual;
        if (GUILayout.Button(portalVisual.IsOpen() ? "Close" : "Open")) {
            portalVisual.SetOpen(!portalVisual.IsOpen());
        }
    }
}
