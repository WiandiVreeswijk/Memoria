using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProgressionNodeComponentReferenceEditor : MonoBehaviour {
    private SerializedObject serializedNode;
    private SerializedProperty onEnterProgressionProperty;
    private SerializedProperty onExitProgressionProperty;
    private ProgressionNodeComponentReference reference;

    public bool DrawGUI(ProgressionNodeComponentReference activeReference) {
        bool changed = false;
        if (reference != activeReference) {
            reference = activeReference;
            OnNodeChange();
        }

        Refresh();

        if (reference == null) return false;
        EditorGUI.BeginChangeCheck();
        bool containsComponent = reference.component != null && serializedNode != null;
        reference.component = EditorGUILayout.ObjectField("Component", reference.component, typeof(ProgressionNodeComponent), true) as ProgressionNodeComponent;
        if (EditorGUI.EndChangeCheck()) {
            if (reference.component == null) {
                reference.scenePath = "";
                reference.ID = "";
                reference.errorMessage = "";
            }
            OnNodeChange();
            changed = true;
        }

        if (containsComponent) GUILayout.Label(reference.component.GetName(), EditorStyles.boldLabel);
        GUILayout.Space(8);


        if (reference.errorMessage.Length > 0) {
            GUILayout.Label(reference.errorMessage, ProgressionDataEditorStyles.ERRORLABEL);
        }
        if (containsComponent) {
            serializedNode.Update();
            EditorGUI.BeginChangeCheck();

            serializedNode.Update();

            var prop = serializedNode.GetIterator();
            prop.NextVisible(true);
            while (prop.NextVisible(false)) {
                EditorGUILayout.PropertyField(prop);
            }

            if (EditorGUI.EndChangeCheck()) {
                serializedNode.ApplyModifiedProperties();
                changed = true;
            }
        }

        return changed;
    }

    void OnNodeChange() {
        if (reference.component != null) {
            Refresh();
            reference.ID = GlobalObjectId.GetGlobalObjectIdSlow(reference.component).ToString();
            reference.scenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        }
    }

    void Refresh() {
        if (reference.component != null) {
            serializedNode = new SerializedObject(reference.component);
        }
    }
}
