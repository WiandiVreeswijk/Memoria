using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NodeBasedEditor : EditorWindow {
    [MenuItem("WAEM/Node Based Editor")]
    private static void OpenWindow() {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Node Based Editor");
    }

    private void OnGUI() {
        DrawNodes();

        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawNodes() {
    }

    private void ProcessEvents(Event e) {
    }
}