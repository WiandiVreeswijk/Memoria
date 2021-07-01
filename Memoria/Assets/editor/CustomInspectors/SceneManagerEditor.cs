using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(SceneManager))]
public class SceneManagerEditor : Editor {
    private ReorderableList list;
    private SerializedProperty sceneDefinitionsProperty;
    private int globalIndex;

    private void OnEnable() {
        sceneDefinitionsProperty = serializedObject.FindProperty("sceneDefinitions");
        list = new ReorderableList(serializedObject, sceneDefinitionsProperty, true, true, true, true);

        list.onAddCallback += AddItem;
        list.drawElementCallback += DrawElement;
        list.drawHeaderCallback += DrawHeader;
    }

    public override void OnInspectorGUI() {
        SceneManager mgr = target as SceneManager;
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        globalIndex = list.index;

        if (globalIndex != -1 && globalIndex < sceneDefinitionsProperty.arraySize) {
            EditorGUILayout.LabelField("Scene properties", EditorStyles.boldLabel);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            serializedObject.Update();
            SerializedProperty sceneDefinition = sceneDefinitionsProperty.GetArrayElementAtIndex(globalIndex);
            SerializedProperty name = sceneDefinition.FindPropertyRelative("name");
            SerializedProperty scene = sceneDefinition.FindPropertyRelative("scene");

            EditorGUILayout.PropertyField(name, new GUIContent("Name"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(scene);
            EditorGUILayout.Space(-28); //Fuck yeah, magic numbers

            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
            //Force override any changes to the prefab for persistence across different scenes.
            PrefabUtility.ApplyPrefabInstance(PrefabUtility.GetNearestPrefabInstanceRoot(mgr.gameObject), InteractionMode.AutomatedAction);
        }
    }

    private void OnDisable() {
        list.onAddCallback -= AddItem;
        list.drawElementCallback -= DrawElement;
        list.drawHeaderCallback -= DrawHeader;
    }

    private void AddItem(ReorderableList list) {
        SceneManager mgr = target as SceneManager;
        mgr.sceneDefinitions.Add(new SceneManager.SceneDefinition());
        EditorUtility.SetDirty(target);
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused) {
        SceneManager mgr = target as SceneManager;
        string name = mgr.sceneDefinitions[index].name;
        if (name.Length == 0) name = "{Empty}";
        GUI.Label(rect, name);
    }

    private void DrawHeader(Rect rect) {
        GUI.Label(rect, "Scenes");
        Rect toggleRect = new Rect(rect.x + rect.width - 20, rect.y, 20, rect.height);
        Rect labelRect = new Rect(rect.x + rect.width - 65, rect.y, 40, rect.height);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleRight;
        GUI.Label(labelRect, "Muted", style);
    }
}