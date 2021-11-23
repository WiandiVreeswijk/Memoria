using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine.UIElements;

public class EditorNode {
    public int id;
    public Rect rect;
    public string name;
    public bool isDragged;
    public bool isSelected;
    public bool isHovered;

    public ConnectionPoint inPoint;
    //public ConnectionPoint outPoint;
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();

    private List<ProgressionNodeComponentReference> sceneNodes = new List<ProgressionNodeComponentReference>();
    private ProgressionNodeComponentReferenceEditor editor = new ProgressionNodeComponentReferenceEditor();
    //public List<ProgressionComponent> onEnterComponents = new List<ProgressionComponent>();
    //public List<ProgressionComponent> onExitComponents = new List<ProgressionComponent>(); 
    //public GameObject gameObject;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    // GUI Style for the title
    public GUIStyle styleID;

    // GUI Style for the fields
    public GUIStyle styleField;

    public Action<EditorNode> OnRemoveNode;

    private ProgressionDataEditorWindow window;

    private static int extraHeight = 350;
    private static int width = 300;
    private static int minNodeHeightCount = 4;
    private static float minHeight = ProgressionDataEditorStyles.TOOLBARHEIGHT + (ConnectionPoint.POINTHEIGHT + ConnectionPoint.OUTPADDING) * minNodeHeightCount + 6;

    private enum ComponentType {
        ENTER,
        EXIT
    }

    public EditorNode(ProgressionDataEditorWindow window, Vector2 position, string name, int id) {
        this.window = window;
        this.name = name;
        this.id = id;

        rect = new Rect(position.x, position.y, width, 100);

        inPoint = new ConnectionPoint(this, "", ConnectionPointType.In, window.OnClickInPoint, null);

        styleID = new GUIStyle();
        styleID.alignment = TextAnchor.UpperCenter;

        styleField = new GUIStyle();
        styleField.alignment = TextAnchor.UpperRight;
        CheckNameLength();
        CheckSceneNodes();
    }

    public void CheckSceneNodes() {
        List<ProgressionNodeComponentReference> toRemove = new List<ProgressionNodeComponentReference>();
        foreach (ProgressionNodeComponentReference reference in sceneNodes) {
            if (reference.IsEmpty()) toRemove.Add(reference);
        }

        foreach (ProgressionNodeComponentReference reference in toRemove) {
            sceneNodes.Remove(reference);
        }

        sceneNodes.Add(new ProgressionNodeComponentReference());

    }

    private void CheckNameLength() {
        if (name.Length == 0) {
            name = "Node " + id;
        }
    }

    Vector2 totalDelta = Vector2.zero;
    Vector2 startDragPos = Vector2.zero;
    public void Drag(Vector2 delta, Vector2 cameraOffset) {
        totalDelta += delta;
        rect.position = startDragPos + totalDelta;
        if (Event.current.shift) {
            rect.position -= cameraOffset;
            float scale = 50f;
            rect.x = Mathf.Round(rect.x / scale) * scale;
            rect.y = Mathf.Round(rect.y / scale) * scale;
            rect.position += cameraOffset;
        }
    }

    public void DragCamera(Vector2 delta) {
        rect.position += delta;
    }

    public void MoveTo(Vector2 pos) {
        rect.position = pos;
    }

    private bool progressionEnterFoldout;
    private bool progressionExitFoldout;
    Vector2 scroll = Vector2.zero;
    public bool Draw(string activeNode) {
        bool changed = false;
        bool isActiveNode = activeNode == name;
        //Calculate height
        float elementHeight = (ConnectionPoint.POINTHEIGHT + ConnectionPoint.OUTPADDING) * outPoints.Count;
        float calculatedHeight = ProgressionDataEditorStyles.TOOLBARHEIGHT + elementHeight + 6;
        calculatedHeight = Mathf.Max(minHeight, calculatedHeight);
        rect.height = calculatedHeight + extraHeight;
        inPoint.Draw(0);

        DrawBox();

        //Outpoints
        for (int i = 0; i < outPoints.Count; i++) outPoints[i].Draw(i);

        GUILayout.BeginArea(rect);

        //Header
        DrawHeader(isActiveNode);

        //Left box
        GUILayout.BeginVertical(GUILayout.Width(rect.width / 2f - 16f));
        GUILayout.Space(6);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        EditorGUI.BeginChangeCheck();
        name = EditorGUILayout.TextField(name, GUILayout.ExpandWidth(true));
        if (EditorGUI.EndChangeCheck()) CheckNameLength();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //Line
        GUI.backgroundColor = ProgressionDataEditorStyles.GREY;
        GUI.Box(new Rect(rect.x, rect.y + calculatedHeight - 1, rect.width, 1), "", ProgressionDataEditorStyles.LINE);
        GUI.backgroundColor = Color.white;

        GUILayout.BeginArea(new Rect(rect.x + 2, rect.y + calculatedHeight, rect.width - 4, rect.height));

        scroll = GUILayout.BeginScrollView(scroll, GUILayout.ExpandWidth(true), GUILayout.Height(extraHeight - 1));
        EditorGUIUtility.labelWidth = 80;

        sceneNodes.ForEach(x => {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (editor.DrawGUI(x)) changed = true;
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
        });
        if (changed) CheckSceneNodes();

        EditorGUIUtility.labelWidth = 150;
        GUILayout.EndScrollView();

        //scroll = GUILayout.BeginScrollView(scroll, GUILayout.ExpandWidth(true), GUILayout.Height(extraHeight));
        //EditorGUIUtility.labelWidth = 130;
        //GUI.backgroundColor = ProgressionDataEditorStyles.DARKGREY;
        //if (progressionEnterFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(progressionEnterFoldout, "On progression enter", ProgressionDataEditorStyles.FOLDOUT)) {
        //    GUI.backgroundColor = Color.white;
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Label("Add component");
        //    GUI.backgroundColor = Color.white;
        //
        //    GUILayout.FlexibleSpace();
        //    if (GUILayout.Button(ProgressionDataEditorStyles.ADDCOMPONENT, ProgressionDataEditorStyles.FOOTERBUTTON)) {
        //        OpenAddComponentMenu(ComponentType.ENTER);
        //
        //    }
        //
        //    GUILayout.EndHorizontal();
        //
        //    //foreach (var c in onEnterComponents) {
        //    //    if (EditorGUILayout.InspectorTitlebar(true, c)) {
        //    //        SerializedObject so = new SerializedObject(c);
        //    //        c.DrawGUI(so);
        //    //    }
        //    //}
        //
        //    GUILayout.Space(EditorGUIUtility.singleLineHeight);
        //} else DrawHorizontalLine();
        //
        //
        //EditorGUILayout.EndFoldoutHeaderGroup();
        //GUI.backgroundColor = ProgressionDataEditorStyles.DARKGREY;
        //if (progressionExitFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(progressionExitFoldout, "On progression exit", ProgressionDataEditorStyles.FOLDOUT)) {
        //    GUI.backgroundColor = Color.white;
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Label("Add component");
        //    GUI.backgroundColor = Color.white;
        //
        //    GUILayout.FlexibleSpace();
        //    if (GUILayout.Button(ProgressionDataEditorStyles.ADDCOMPONENT, ProgressionDataEditorStyles.FOOTERBUTTON)) {
        //        OpenAddComponentMenu(ComponentType.EXIT);
        //    }
        //
        //    GUILayout.EndHorizontal();
        //
        //    //foreach (var c in onExitComponents) {
        //    //    if (EditorGUILayout.InspectorTitlebar(true, c)) {
        //    //        SerializedObject so = new SerializedObject(c);
        //    //        c.DrawGUI(so);
        //    //    }
        //    //}
        //    GUILayout.Space(EditorGUIUtility.singleLineHeight);
        //}
        //EditorGUILayout.EndFoldoutHeaderGroup();
        //GUI.backgroundColor = Color.white;



        //gameObject= EditorGUILayout.ObjectField("A", gameObject, typeof(GameObject), true) as GameObject;

        //GUILayout.EndScrollView();
        GUILayout.EndArea();
        return changed;
    }

    private void OpenAddComponentMenu(ComponentType componentType) {
        GenericMenu genericMenu = new GenericMenu();
        var types = ProgressionUtils.GetEnumerableOfType<ProgressionNodeComponent>();
        foreach (Type type in types) {
            genericMenu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(type.Name)), false, () => {
                //if (componentType == ComponentType.ENTER) onEnterComponents.Add((ProgressionComponent)Activator.CreateInstance(type));
                //else if (componentType == ComponentType.EXIT) onExitComponents.Add((ProgressionComponent)Activator.CreateInstance(type));
            });
        }

        genericMenu.ShowAsContext();
    }

    private void DrawHorizontalLine() {
        GUI.backgroundColor = ProgressionDataEditorStyles.GREY;
        GUILayout.Box("", ProgressionDataEditorStyles.LINE, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        GUI.backgroundColor = Color.white;
    }

    private void DrawBox() {
        //if (isSelected) GUI.backgroundColor = Color.white * 1.5f;
        GUI.Box(rect, name, GUI.skin.window);
        GUI.Box(new Rect(rect.x, rect.y, rect.width / 2f, rect.height - extraHeight), GUIContent.none, EditorStyles.helpBox);
        GUI.backgroundColor = Color.white;
    }

    private void DrawHeader(bool isActiveNode) {
        if (IsBaseNode()) GUI.backgroundColor = ProgressionDataEditorStyles.BASENODE;
        if (isActiveNode) GUI.backgroundColor = ProgressionDataEditorStyles.ACTIVENODE;
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label(name + (isActiveNode ? " (active)" : ""));
        GUI.backgroundColor = Color.white;

        GUILayout.FlexibleSpace();
        if (GUILayout.Button(ProgressionDataEditorStyles.ADDOUTPUTNODE, ProgressionDataEditorStyles.FOOTERBUTTON)) {
            AddOutPoint();
        }

        GUILayout.EndHorizontal();
    }

    public bool ProcessEvents(Event e, Vector2 cameraOffset) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    if (rect.Contains(e.mousePosition)) {
                        isDragged = true;
                        totalDelta = Vector2.zero;
                        startDragPos = rect.position;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && rect.Contains(e.mousePosition)) {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseMove:
                isHovered = rect.Contains(e.mousePosition);
                break;
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged) {
                    Drag(e.delta, cameraOffset);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu() {
        GenericMenu genericMenu = new GenericMenu();
        if (!IsBaseNode())
            genericMenu.AddItem(new GUIContent("Set base node"), false, OnClickSetBaseNode);
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private bool IsBaseNode() {
        return window.GetBaseNodeID() == id;
    }

    private void OnClickSetBaseNode() {
        window.SetBaseNode(this);
    }

    public void AddOutPoint(string name = "") {
        outPoints.Add(new ConnectionPoint(this, name, ConnectionPointType.Out, window.OnClickOutPoint, DeleteOutNode));
    }

    private void DeleteOutNode(ConnectionPoint point) {
        window.OnClickRemoveConnection(point);
        outPoints.Remove(point);
    }

    private void OnClickRemoveNode() {
        window.OnClickRemoveNode(this);
    }

    public void InitializeSceneNodes() {
        sceneNodes.ForEach(x => x.Initialize());
    }

    public void AddSceneNode(ProgressionNodeComponentReference progressionNode) {
        sceneNodes.Add(progressionNode);
    }

    public List<ProgressionNodeComponentReference> GetUsedSceneNodes() {
        List<ProgressionNodeComponentReference> references = new List<ProgressionNodeComponentReference>();
        foreach (var reference in sceneNodes) {
            if (!reference.IsEmpty()) {
                references.Add(reference);
            }
        }

        return references;

    }
}