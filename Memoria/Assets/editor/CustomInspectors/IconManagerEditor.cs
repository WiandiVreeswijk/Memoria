using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(IconManager))]
public class IconManagerEditor : Editor {
    private ReorderableList iconsList;
    private SerializedProperty iconDefinitionsProperty;
    private int globalIndex;

    private void OnEnable() {
        iconDefinitionsProperty = serializedObject.FindProperty("iconDefinitions");
        iconsList = new ReorderableList(serializedObject, iconDefinitionsProperty, true, true, true, true);
        iconsList.onAddCallback += AddItem;
        iconsList.drawElementCallback += DrawElement;
        iconsList.drawHeaderCallback += DrawHeader;
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        iconsList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        globalIndex = iconsList.index;
        if (globalIndex != -1 && globalIndex < iconDefinitionsProperty.arraySize) {
            EditorGUILayout.LabelField("Icon properties", EditorStyles.boldLabel);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            serializedObject.Update();
            SerializedProperty sceneDefinition = iconDefinitionsProperty.GetArrayElementAtIndex(globalIndex);
            SerializedProperty nameProp = sceneDefinition.FindPropertyRelative("name");
            SerializedProperty imageProp = sceneDefinition.FindPropertyRelative("image");

            EditorGUILayout.PropertyField(nameProp, new GUIContent("Name"));
            EditorGUILayout.PropertyField(imageProp);

            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
        }
    }

    private void OnDisable() {
        iconsList.onAddCallback -= AddItem;
        iconsList.drawElementCallback -= DrawElement;
        iconsList.drawHeaderCallback -= DrawHeader;
    }

    private void AddItem(ReorderableList list) {
        IconManager mgr = target as IconManager;
        mgr.iconDefinitions.Add(new IconManager.IconDefinition());
        EditorUtility.SetDirty(target);
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused) {
        IconManager mgr = target as IconManager;
        string name = mgr.iconDefinitions[index].name;
        if (name.Length == 0) name = "{Empty}";
        GUI.Label(rect, name);
    }

    private void DrawHeader(Rect rect) {
        GUI.Label(rect, "Icons");
    }
}