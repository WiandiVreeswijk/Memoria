using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;

public class ProgressionDataEditorWindow : EditorWindow {
    private const string TITLE = "Progression Data Editor";

    private List<Node> nodes = new List<Node>();
    private List<Connection> connections = new List<Connection>();

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    private ProgressionData progressionData;
    //private SerializedObject serializedObject;

    private int nodeID;
    private bool dirty = false;

    [UnityEditor.Callbacks.OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line) {
        bool isProgressionData = EditorUtility.InstanceIDToObject(instanceID).GetType() == typeof(ProgressionData);
        ProgressionData progressionData = Selection.activeObject as ProgressionData;
        if (isProgressionData && progressionData != null) {
            OpenWindow(progressionData);
            return true;
        }
        return false;
    }

    public static void OpenWindow(ProgressionData progressionData) {
        ProgressionDataEditorWindow window = GetWindow<ProgressionDataEditorWindow>();
        window.titleContent = new GUIContent(TITLE);
        window.SelectData(progressionData);
    }


    [MenuItem("WAEM/Node Based Editor")]
    private static void OpenWindow() {
        ProgressionDataEditorWindow window = GetWindow<ProgressionDataEditorWindow>();
        window.titleContent = new GUIContent(TITLE);
    }

    private void OnEnable() {
        LoadProgressionData();
    }

    public void OnDestroy() {
        if (dirty) {
            string path = AssetDatabase.GetAssetPath(progressionData);
            if (EditorUtility.DisplayDialog("Procession Data Has Been Modified",
                $"Do you want to save the changes you made to the Progression Data?\n\n{path}\n\nYour changes will be lost if you don't save them.",
                "Save", "Discard changes")) {
                SaveProgressionData();
            }
        }
    }

    string activeNode = "";
    bool shouldRepaint = false;
    private void Update() {
        if (!Application.isPlaying) return;
        string previousNode = activeNode;
        if (ProgressionManager._Instance != null && ProgressionManager._Instance.progressionData == progressionData) {
            activeNode = ProgressionManager._Instance.GetActiveNode().name;
        } else activeNode = "";

        if (activeNode != previousNode)
            Repaint();
    }

    private void OnGUI() {
        ProgressionDataEditorStyles.InitStyles();
        //EditorGUI.BeginDisabledGroup(Application.isPlaying);
        //Matrix4x4 before = GUI.matrix;
        //GUIUtility.ScaleAroundPivot(Vector2.one * zoomScale, new Vector2(position.width, position.height) / 2.0f);

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        if (progressionData != null) {
            //serializedObject.Update();
            DrawNodes(activeNode);
            DrawConnections();

            DrawConnectionLine(Event.current);
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            //serializedObject.ApplyModifiedProperties();
        }

        //GUI.matrix = before;

        DrawToolbar();
        DrawProgressionDataField();

        if (GUI.changed) Repaint();
        //EditorGUI.EndDisabledGroup();
    }

    private void DrawToolbar() {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("Save Asset", EditorStyles.toolbarButton)) {
            SaveProgressionData();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawProgressionDataField() {
        EditorGUILayout.BeginHorizontal();
        var newData = EditorGUILayout.ObjectField(progressionData, typeof(ProgressionData), false) as ProgressionData;
        if (newData != progressionData) SelectData(newData);
        //DrawGearMenu();
        EditorGUILayout.EndHorizontal();
    }

    private void SelectData(ProgressionData newData) {
        ClearNodes();
        progressionData = newData;
        //serializedObject = (newData != null) ? new SerializedObject(newData) : null;
        LoadProgressionData();
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

    private void DrawNodes(string activeNode) {
        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Draw(activeNode);
            }
        }
    }

    private void DrawConnections() {
        if (connections != null) {
            for (int i = 0; i < connections.Count; i++) {
                connections[i].Draw();
            }
        }
    }

    private void MarkDirty() {
        dirty = true;
        titleContent.text = TITLE + "*";
    }

    private void ProcessEvents(Event e) {
        drag = Vector2.zero;

        Rect rect = position;
        rect.x = 0;
        rect.y = 0;
        rect.y += ProgressionDataEditorStyles.TOOLBARHEIGHT * 2;
        rect.height -= ProgressionDataEditorStyles.TOOLBARHEIGHT * 2;
        bool mouseWithinGrid = rect.Contains(e.mousePosition);
        switch (e.type) {
            case EventType.MouseDown:
                if (mouseWithinGrid) {
                    GUI.FocusControl("");
                    if (e.button == 0) {
                        ClearConnectionSelection();
                    }

                    if (e.button == 1) {
                        ProcessContextMenu(e.mousePosition);
                    }
                }
                break;
            case EventType.MouseDrag:
                if (mouseWithinGrid) {
                    if (e.button == 0) {
                        OnDrag(e.delta);
                    }
                }
                break;
            case EventType.ScrollWheel:
                if (mouseWithinGrid) {
                    //if (e.delta.y > 0) zoomScale /= 1.2f;
                    //else zoomScale *= 1.2f;
                    //zoomScale = Mathf.Clamp(zoomScale, 0.1f, 100f);
                    //GUI.changed = true;
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e) {
        if (nodes != null) {
            for (int i = nodes.Count - 1; i >= 0; i--) {
                bool guiChanged = nodes[i].ProcessEvents(e, totalDrag);

                if (guiChanged) {
                    GUI.changed = true;
                    MarkDirty();
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

    private Vector2 totalDrag = Vector2.zero;
    private void OnDrag(Vector2 delta) {
        totalDrag += delta;
        drag = delta;

        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].DragCamera(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddNode(Vector2 mousePosition) {
        // We create the node with the default info for the node.
        int id = nodeID++;
        Node node = new Node(this, mousePosition, "", id);
        node.AddOutPoint();
        nodes.Add(node);

        if (nodes.Count == 1) {
            node.isBaseNode = true;
        }
        MarkDirty();
    }

    private void ClearNodes() {
        //nodeCount = 0;
        if (nodes != null && nodes.Count > 0) {
            Node node;
            while (nodes.Count > 0) {
                node = nodes[0];
                OnClickRemoveNode(node);
            }
        }

        if (connections.Count > 0) {
            Connection connection;
            while (connections.Count > 0) {
                connection = connections[0];
                OnClickRemoveConnection(connection);
            }
        }
    }

    public void OnClickInPoint(ConnectionPoint inPoint) {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
                MarkDirty();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    public void OnClickOutPoint(ConnectionPoint outPoint) {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
                MarkDirty();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    public void OnClickRemoveNode(Node node) {
        if (connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++) {
                if (connections[i].inPoint == node.inPoint || node.outPoints.Contains(connections[i].outPoint)) {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++) {
                connections.Remove(connectionsToRemove[i]);
            }
        }

        nodes.Remove(node);
        MarkDirty();
    }

    public void OnClickRemoveConnection(ConnectionPoint point) {
        if (connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++) {
                if (connections[i].inPoint == point || connections[i].outPoint == point) {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++) {
                connections.Remove(connectionsToRemove[i]);
            }
        }

        MarkDirty();
    }

    public void OnClickRemoveConnection(Connection connection) {
        connections.Remove(connection);
        MarkDirty();
    }

    private void CreateConnection() {
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        MarkDirty();
    }

    private void ClearConnectionSelection() {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    #region LoadingAndSaving
    private void LoadProgressionData() {
        if (progressionData == null) return;
        Dictionary<int, Node> nodesDict = new Dictionary<int, Node>();
        nodeID = progressionData.nodeID;
        // Create nodes
        for (int i = 0; i < progressionData.nodeDataCollection.Length; i++) {
            ProgressionData.NodeData data = progressionData.nodeDataCollection[i];
            Node node = new Node(this, data.position, data.name, data.id);

            foreach (var nodeConnection in data.connections) {
                node.AddOutPoint(nodeConnection.name);
            }

            //node.onExitComponents = new List<ProgressionComponentBase>(data.onExitComponents);
            //node.onEnterComponents = new List<ProgressionComponentBase>(data.onEnterComponents);
            node.gameObject = data.gameObject;
            node.isBaseNode = data.isBaseNode;
            nodes.Add(node);
            nodesDict.Add(data.id, node);
        }


        for (int i = 0; i < progressionData.nodeDataCollection.Length; i++) {
            ProgressionData.NodeData data = progressionData.nodeDataCollection[i];
            for (int j = 0; j < data.connections.Length; j++) {
                Node node = nodesDict[data.id];
                if (data.connections[j].reference != -1) {
                    connections.Add(new Connection(nodesDict[data.connections[j].reference].inPoint, node.outPoints[j], OnClickRemoveConnection));
                }
            }
        }
    }

    public void SetBaseNode(Node newBaseNode) {
        if (newBaseNode.isBaseNode) return;
        foreach (var node in nodes) {
            node.isBaseNode = false;
        }
        newBaseNode.isBaseNode = true;
        MarkDirty();
    }

    private void SaveProgressionData() {
        if (progressionData != null) {
            StoreNodeData();
            EditorUtility.SetDirty(progressionData);
            AssetDatabase.SaveAssetIfDirty(progressionData);
            dirty = false;
            titleContent.text = TITLE;
        }
    }

    private void StoreNodeData() {
        progressionData.nodeID = nodeID;
        progressionData.nodeDataCollection = new ProgressionData.NodeData[nodes.Count];

        for (int i = 0; i < nodes.Count; ++i) {
            Node node = nodes[i];

            progressionData.nodeDataCollection[i] = new ProgressionData.NodeData();
            progressionData.nodeDataCollection[i].id = node.id;
            progressionData.nodeDataCollection[i].name = node.name;
            progressionData.nodeDataCollection[i].position = node.rect.position - totalDrag;
            progressionData.nodeDataCollection[i].isBaseNode = node.isBaseNode;
            progressionData.nodeDataCollection[i].gameObject= node.gameObject;
            //progressionData.nodeDataCollection[i].onExitComponents = node.onExitComponents.ToArray();
            //progressionData.nodeDataCollection[i].onEnterComponents = node.onEnterComponents.ToArray();
            progressionData.nodeDataCollection[i].connections = new ProgressionData.NodeConnection[node.outPoints.Count];
            for (int j = 0; j < node.outPoints.Count; j++) {
                var outPoint = node.outPoints[j];
                progressionData.nodeDataCollection[i].connections[j] = new ProgressionData.NodeConnection();
                progressionData.nodeDataCollection[i].connections[j].name = outPoint.name;
                progressionData.nodeDataCollection[i].connections[j].reference = -1;
                for (int k = 0; k < connections.Count; k++) {
                    if (connections[k].outPoint == outPoint) {
                        progressionData.nodeDataCollection[i].connections[j].reference = connections[k].inPoint.node.id;
                    }
                }
            }
        }
    }
    #endregion
}