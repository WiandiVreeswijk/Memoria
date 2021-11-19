using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProgressionSceneNodeReferenceEditor : MonoBehaviour {
    private SerializedObject serializedNode;
    private SerializedProperty onEnterProgressionProperty;
    private SerializedProperty onExitProgressionProperty;
    private ProgressionSceneNodeReference reference;

    public bool DrawGUI(ProgressionSceneNodeReference activeReference) {
        bool changed = false;
        if (reference != activeReference) {
            reference = activeReference;
            OnNodeChange();
        }

        Refresh();

        if (reference == null) return false;
        EditorGUI.BeginChangeCheck();
        reference.node = EditorGUILayout.ObjectField("Scene Node", reference.node, typeof(ProgressionSceneNode), true) as ProgressionSceneNode;
        if (EditorGUI.EndChangeCheck()) {
            if (reference.node == null) {
                reference.scenePath = "";
                reference.ID = "";
                reference.errorMessage = "";
            }
            OnNodeChange();
            changed = true;
        }
        GUILayout.Space(8);

        if (reference.errorMessage.Length > 0) {
            GUILayout.Label(reference.errorMessage, ProgressionDataEditorStyles.ERRORLABEL);
        }

        if (reference.node != null && serializedNode != null) {
            serializedNode.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(onEnterProgressionProperty);
            EditorGUILayout.PropertyField(onExitProgressionProperty);
            serializedNode.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck()) {
                changed = true;
            }
        }

        return changed;
    }

    void OnNodeChange() {
        if (reference.node != null) {
            Refresh();
            reference.ID = GlobalObjectId.GetGlobalObjectIdSlow(reference.node).ToString();
            reference.scenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        }
    }

    void Refresh() {
        if (reference.node != null) {
            serializedNode = new SerializedObject(reference.node);
            onEnterProgressionProperty = serializedNode.FindProperty("onEnterNode");
            onExitProgressionProperty = serializedNode.FindProperty("onExitNode");
        }
    }
}
