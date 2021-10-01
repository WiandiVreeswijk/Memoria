using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(MenuCamera))]
public class MenuCameraEditor : Editor {
    private ReorderableList pointsList;
    private SerializedProperty menuCameraPointsProperty;
    private int globalIndex;

    private void OnEnable() {
        menuCameraPointsProperty = serializedObject.FindProperty("menuCameraPoints");
        pointsList = new ReorderableList(serializedObject, menuCameraPointsProperty, true, true, true, true);
        pointsList.onAddCallback += AddItem;
        pointsList.drawElementCallback += DrawElement;
        pointsList.drawHeaderCallback += DrawHeader;
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        pointsList.DoLayoutList();
        globalIndex = pointsList.index;
        if (globalIndex != -1 && globalIndex < menuCameraPointsProperty.arraySize) {
            EditorGUILayout.LabelField("Point", EditorStyles.boldLabel);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            serializedObject.Update();
            SerializedProperty menuCameraPoint = menuCameraPointsProperty.GetArrayElementAtIndex(globalIndex);
            SerializedProperty nameProperty = menuCameraPoint.FindPropertyRelative("name");
            SerializedProperty positionProperty = menuCameraPoint.FindPropertyRelative("position");
            SerializedProperty rotationProperty = menuCameraPoint.FindPropertyRelative("rotation");

            EditorGUILayout.PropertyField(nameProperty);
            EditorGUILayout.PropertyField(positionProperty);
            EditorGUILayout.PropertyField(rotationProperty);
            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }

    private void OnDisable() {
        pointsList.onAddCallback -= AddItem;
        pointsList.drawElementCallback -= DrawElement;
        pointsList.drawHeaderCallback -= DrawHeader;
    }

    private void AddItem(ReorderableList list) {
        MenuCamera menuCamera = target as MenuCamera;
        Camera camera = SceneView.lastActiveSceneView.camera;

        menuCamera.menuCameraPoints.Add(new MenuCamera.MenuCameraPoint(camera.transform.position, camera.transform.rotation));
        EditorUtility.SetDirty(target);
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused) {
        MenuCamera menuCamera = target as MenuCamera;
        string name = menuCamera.menuCameraPoints[index].name;
        if (name.Length == 0) name = "{Empty}";

        Rect newRect = new Rect(rect.x, rect.y, rect.width / 2.0f, rect.height);
        Rect newRect2 = new Rect(rect.x + rect.width / 2.0f, rect.y, rect.width / 2.0f, rect.height);

        GUI.Label(newRect, name);
        if (GUI.Button(newRect2, "Go to"))
        {
            var view = SceneView.lastActiveSceneView;
            if (view != null) {
                var target = new GameObject();
                target.transform.position = menuCamera.menuCameraPoints[index].position;
                target.transform.rotation = menuCamera.menuCameraPoints[index].rotation;
                view.AlignViewToObject(target.transform);
                GameObject.DestroyImmediate(target);
            }
        }
    }

    private void DrawHeader(Rect rect) {
        GUI.Label(rect, "Points");
    }
}