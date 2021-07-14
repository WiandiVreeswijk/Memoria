using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(SceneManager))]
public class SceneManagerEditor : Editor {
    private ReorderableList listKey;
    private ReorderableList listLevels;
    private SerializedProperty wijkSceneProperty;
    private SerializedProperty levelsceneDefinitionsProperty;
    private SerializedProperty keySceneDefinitionsProperty;
    private int globalIndex;

    private void OnEnable() {
        wijkSceneProperty = serializedObject.FindProperty("wijkScene");
        levelsceneDefinitionsProperty = serializedObject.FindProperty("levelSceneDefinitions");
        keySceneDefinitionsProperty = serializedObject.FindProperty("keySceneDefinitions");
        listKey = new ReorderableList(serializedObject, keySceneDefinitionsProperty, true, true, true, true);
        listKey.onAddCallback += AddItemKey;
        listKey.drawElementCallback += DrawElementKey;
        listKey.drawHeaderCallback += DrawHeaderKey;
        listKey.onSelectCallback += OnSelectKey;

        listLevels = new ReorderableList(serializedObject, levelsceneDefinitionsProperty, true, true, true, true);
        listLevels.onAddCallback += AddItemLevels;
        listLevels.drawElementCallback += DrawElementLevels;
        listLevels.drawHeaderCallback += DrawHeaderLevels;
        listLevels.onSelectCallback += OnSelectLevels;
    }

    private bool listKeySelected = false;
    private void OnSelectKey(ReorderableList list) {
        listKeySelected = true;
    }

    private void OnSelectLevels(ReorderableList list) {
        listKeySelected = false;
    }


    public override void OnInspectorGUI() {
        SceneManager mgr = target as SceneManager;
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        Rect rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);

        SerializedProperty wijkName = wijkSceneProperty.FindPropertyRelative("name");
        SerializedProperty wijkScene = wijkSceneProperty.FindPropertyRelative("scene");
        SerializedProperty wijkSkybox = wijkSceneProperty.FindPropertyRelative("skybox");
        SerializedProperty wijkFog = wijkSceneProperty.FindPropertyRelative("fog");
        EditorGUILayout.PropertyField(wijkName);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(wijkFog);
        EditorGUILayout.PropertyField(wijkSkybox);
        EditorGUILayout.PropertyField(wijkScene);
        EditorGUILayout.Space(-28);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Scenes");
        listKey.DoLayoutList();
        listLevels.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        globalIndex = listKeySelected ? listKey.index : listLevels.index;
        SerializedProperty prop = listKeySelected ? keySceneDefinitionsProperty : levelsceneDefinitionsProperty;
        if (globalIndex != -1 && globalIndex < prop.arraySize) {
            EditorGUILayout.LabelField("Scene properties", EditorStyles.boldLabel);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            serializedObject.Update();
            SerializedProperty sceneDefinition = prop.GetArrayElementAtIndex(globalIndex);
            SerializedProperty name = sceneDefinition.FindPropertyRelative("name");
            SerializedProperty scene = sceneDefinition.FindPropertyRelative("scene");
            SerializedProperty skybox = sceneDefinition.FindPropertyRelative("skybox");
            SerializedProperty fog = sceneDefinition.FindPropertyRelative("fog");

            EditorGUILayout.PropertyField(name, new GUIContent("Name"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(fog);
            EditorGUILayout.PropertyField(skybox);
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
        listKey.onAddCallback -= AddItemKey;
        listKey.drawElementCallback -= DrawElementKey;
        listKey.drawHeaderCallback -= DrawHeaderKey;
        listKey.onSelectCallback -= OnSelectKey;

        listLevels.onAddCallback -= AddItemLevels;
        listLevels.drawElementCallback -= DrawElementLevels;
        listLevels.drawHeaderCallback -= DrawHeaderLevels;
        listLevels.onSelectCallback -= OnSelectLevels;
    }

    private void AddItemKey(ReorderableList list) {
        SceneManager mgr = target as SceneManager;
        mgr.keySceneDefinitions.Add(new SceneManager.SceneDefinition());
        EditorUtility.SetDirty(target);
    }

    private void AddItemLevels(ReorderableList list) {
        SceneManager mgr = target as SceneManager;
        mgr.levelSceneDefinitions.Add(new SceneManager.SceneDefinition());
        EditorUtility.SetDirty(target);
    }

    private void DrawElementKey(Rect rect, int index, bool active, bool focused) {
        SceneManager mgr = target as SceneManager;
        string name = mgr.keySceneDefinitions[index].name;
        if (name.Length == 0) name = "{Empty}";
        GUI.Label(rect, name);
    }

    private void DrawElementLevels(Rect rect, int index, bool active, bool focused) {
        SceneManager mgr = target as SceneManager;
        string name = mgr.levelSceneDefinitions[index].name;
        if (name.Length == 0) name = "{Empty}";
        GUI.Label(rect, name);
    }

    private void DrawHeaderKey(Rect rect) {
        GUI.Label(rect, "Key scenes");
    }

    private void DrawHeaderLevels(Rect rect) {
        GUI.Label(rect, "Level scenes");
    }
}