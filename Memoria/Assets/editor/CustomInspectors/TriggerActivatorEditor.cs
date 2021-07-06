using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
//[CustomEditor(typeof(TriggerActivator))]
//public class TriggerActivatorEditor : Editor {
//    SerializedProperty triggerName;
//    SerializedProperty activatables;
//    SerializedProperty activatablesAttachedToThisObject;
//
//    void OnEnable() {
//        serializedObject.Update();
//        triggerName = serializedObject.FindProperty("triggerName");
//        activatables = serializedObject.FindProperty("activatables");
//        activatablesAttachedToThisObject = serializedObject.FindProperty("activatablesAttachedToThisObject");
//    }
//
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        EditorGUILayout.PropertyField(triggerName);
//        EditorGUILayout.PropertyField(activatablesAttachedToThisObject);
//        //if (!activatablesAttachedToThisObject.boolValue) {
//           EditorGUILayout.PropertyField(activatables);
//        //}
//        serializedObject.ApplyModifiedProperties();
//    }
//}