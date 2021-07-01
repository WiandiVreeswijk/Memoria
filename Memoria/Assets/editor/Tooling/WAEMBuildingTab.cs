using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[Serializable]
public class WAEMBuildingTab : IWAEMTab {
    public void Initialize() {

    }

    [SerializeField]
    private GameObject prefab;
    private bool isDragPerformed = false;
    private bool isDragging = false;

    public void OnGUI(EditorWindow window, GUIStyle style) {
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), false) as GameObject;

        //if (prefab == null) return;
        //var box = new VisualElement();
        //box.style.backgroundColor = Color.red;
        //// box.style.flexGrow = 1f;
        //
        //box.RegisterCallback<MouseDownEvent>(evt => {
        //    DragAndDrop.PrepareStartDrag();
        //    DragAndDrop.StartDrag("Dragging");
        //    DragAndDrop.objectReferences = new Object[] { prefab };
        //
        //    Selection.activeGameObject = null;
        //    isDragPerformed = false;
        //    isDragging = true;
        //});
        //
        //box.RegisterCallback<DragUpdatedEvent>(evt => {
        //    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        //});
        //
        //window.rootVisualElement.Add(box);
        //SceneView.duringSceneGui += sv => OnDragEnd();
        //EditorApplication.hierarchyWindowItemOnGUI += (id, rect) => OnDragEnd();
    }

    private void OnDragEnd() {
        if (Event.current.type == EventType.DragPerform) {
            isDragPerformed = true;
        }

        if (Event.current.type == EventType.DragExited) {
            if (isDragging && isDragPerformed) {
                isDragging = false;
                isDragPerformed = false;

                var go = Selection.activeGameObject;
                // Do your **OnDragEnd callback on go** here
            }
        }
    }


    public void OnUpdate() {

    }

    public void OnSelectionChange(EditorWindow window) {

    }

    public void OnDestroy() {

    }
}
