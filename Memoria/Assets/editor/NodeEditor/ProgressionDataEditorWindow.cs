using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ProgressionDataEditorWindow : EditorWindow {
    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    private ProgressionData progressionData;

    [UnityEditor.Callbacks.OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line) {
        ProgressionData progressionData = Selection.activeObject as ProgressionData;
        if (progressionData != null) {
            ProgressionDataEditorWindow window = GetWindow<ProgressionDataEditorWindow>();
            window.titleContent = new GUIContent("Progression Data");
            window.progressionData = progressionData;
            return true;
        }
        return false;
    }

    [MenuItem("WAEM/Node Based Editor")]
    private static void OpenWindow() {
        ProgressionDataEditorWindow window = GetWindow<ProgressionDataEditorWindow>();
        window.titleContent = new GUIContent("Progression Data");
    }

    private void OnEnable() {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    private void OnGUI() {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        if (progressionData != null) {
            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
        }
        //GUILayout.BeginHorizontal(EditorStyles.toolbar);
        //GUILayout.Toggle(false, "Toggle Me", EditorStyles.toolbarButton);
        //GUILayout.FlexibleSpace();
        //GUILayout.EndHorizontal();

        DrawProgressionDataField();



        if (GUI.changed) Repaint();
    }

    private void DrawProgressionDataField() {
        EditorGUILayout.BeginHorizontal();
        var newData = EditorGUILayout.ObjectField(progressionData, typeof(ProgressionData), false) as ProgressionData;
        if (newData != progressionData) SelectData(newData);
        //DrawGearMenu();
        EditorGUILayout.EndHorizontal();
    }

    private void SelectData(ProgressionData newData) {
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
        //if (offset.x < 0) {
        //    widthDivs+=10;
        //    heightDivs+=10;
        //}

        if (offset.y < 0) {
            widthDivs++;
            heightDivs++;
        }
        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height + (offset.y < 0 ? gridSpacing : 0), 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width + (offset.x < 0 ? gridSpacing : 0), gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes() {
        if (progressionData.nodes != null) {
            for (int i = 0; i < progressionData.nodes.Count; i++) {
                progressionData.nodes[i].Draw();
            }
        }
    }

    private void DrawConnections() {
        if (progressionData.connections != null) {
            for (int i = 0; i < progressionData.connections.Count; i++) {
                progressionData.connections[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e) {
        drag = Vector2.zero;

        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    ClearConnectionSelection();
                }

                if (e.button == 1) {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0) {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e) {
        if (progressionData.nodes != null) {
            for (int i = progressionData.nodes.Count - 1; i >= 0; i--) {
                bool guiChanged = progressionData.nodes[i].ProcessEvents(e);

                if (guiChanged) {
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionLine(Event e) {
        if (selectedInPoint != null && selectedOutPoint == null) {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null) {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition) {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta) {
        drag = delta;

        if (progressionData.nodes != null) {
            for (int i = 0; i < progressionData.nodes.Count; i++) {
                progressionData.nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddNode(Vector2 mousePosition) {
        if (progressionData.nodes == null) {
            progressionData.nodes = new List<Node>();
        }

        // We create the node with the default info for the node.
        progressionData.nodes.Add(new Node(mousePosition, 200, 100, nodeStyle, selectedNodeStyle,
            inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
            0, false, 0, null));
    }

    private void OnClickInPoint(ConnectionPoint inPoint) {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint) {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node) {
        if (progressionData.connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < progressionData.connections.Count; i++) {
                if (progressionData.connections[i].inPoint == node.inPoint || progressionData.connections[i].outPoint == node.outPoint) {
                    connectionsToRemove.Add(progressionData.connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++) {
                progressionData.connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        progressionData.nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection) {
        progressionData.connections.Remove(connection);
    }

    private void CreateConnection() {
        if (progressionData.connections == null) {
            progressionData.connections = new List<Connection>();
        }

        progressionData.connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection() {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}