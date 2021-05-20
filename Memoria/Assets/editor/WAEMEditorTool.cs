using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public class WAEMEditorTool : EditorWindow {


    static WAEMEditorTool() {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView) {

        Handles.BeginGUI();
        Rect windowRect = new Rect(0, 25, 180, EditorGUIUtility.singleLineHeight * 2f);
        GUILayout.Window(666, windowRect, DrawWindowForSpriteRenderer, "Satellite Tool");
        Handles.EndGUI();
    }

    private static GameObject prefab;
    private static bool m_IsDragPerformed = false;
    private static bool m_IsDragging = false;

    private static GUIContent _content;
    static void DrawWindowForSpriteRenderer(int windowID) {
        //GUIStyle style = new GUIStyle(GUI.skin.button);
        //var prefabs = GetAllPrefabs();
        prefab = EditorGUILayout.ObjectField((Object)prefab, typeof(GameObject), false) as GameObject;

        ////foreach (var a in prefabs) {
        ////    string assetPath = AssetDatabase.GUIDToAssetPath(a);
        ////    Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
        ////
        //_content = new GUIContent("Click me", (Texture)AssetPreview.GetAssetPreview(obj));
        //if (GUILayout.Button(
        //    _content)) {
        //    objectShouldFollowMouse = true;
        //
        //    objectThatFollowsMouse = Instantiate(obj) as GameObject;
        //}
        //
        //if (objectThatFollowsMouse == null) {
        //    objectShouldFollowMouse = false;
        //}
        //
        //GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        //buttonStyle.margin = new RectOffset(0, 5, 2, 0);
        //
        //Vector3 mousePoint = Camera.main.ViewportToWorldPoint(Input.mousePosition);
        //if (objectShouldFollowMouse) {
        //    objectThatFollowsMouse.transform.position = mousePoint;
        //    //if (Event.current.type == EventType.MouseDown) {
        //    //    objectShouldFollowMouse = false;
        //    //    objectThatFollowsMouse = null;
        //    //}
        //}
        if(prefab == null) return;
        var box = new VisualElement();
        box.style.backgroundColor = Color.red;
        box.style.flexGrow = 1f;

        box.RegisterCallback<MouseDownEvent>(evt => {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.StartDrag("Dragging");
            DragAndDrop.objectReferences = new Object[] { prefab };

            Selection.activeGameObject = null;
            m_IsDragPerformed = false;
            m_IsDragging = true;
        });

        box.RegisterCallback<DragUpdatedEvent>(evt => {
            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        });

       // rootVisualElement.Add(box);
        SceneView.duringSceneGui += sv => OnDragEnd();
        EditorApplication.hierarchyWindowItemOnGUI += (id, rect) => OnDragEnd();
    }

    private static void OnDragEnd() {
        if (Event.current.type == EventType.DragPerform) {
            m_IsDragPerformed = true;
        }

        if (Event.current.type == EventType.DragExited) {
            if (m_IsDragging && m_IsDragPerformed) {
                m_IsDragging = false;
                m_IsDragPerformed = false;

                var go = Selection.activeGameObject;
                // Do your **OnDragEnd callback on go** here
            }
        }
    }

    public static string[] GetAllPrefabs() {
        string[] temp = AssetDatabase.GetAllAssetPaths();
        List<string> result = new List<string>();
        foreach (string s in temp) {
            if (s.Contains(".prefab")) result.Add(s);
        }
        return result.ToArray();
    }
}