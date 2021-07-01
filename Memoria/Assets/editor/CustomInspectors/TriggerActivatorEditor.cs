using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(TriggerActivator))]
public class TriggerActivatorEditor : Editor {
    SerializedProperty triggerName;
    SerializedProperty activatable;
    SerializedProperty activatablesAttachedToThisObject;

    void OnEnable() {
        triggerName = serializedObject.FindProperty("triggerName");
        activatable = serializedObject.FindProperty("activatable");
        activatablesAttachedToThisObject = serializedObject.FindProperty("activatablesAttachedToThisObject");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(triggerName);
        EditorGUILayout.PropertyField(activatablesAttachedToThisObject);
        if (!activatablesAttachedToThisObject.boolValue) {
           EditorGUILayout.PropertyField(activatable);
        }
        serializedObject.ApplyModifiedProperties();
    }
}