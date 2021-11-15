using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ProgressionComponent : ProgressionComponentBase {

    public override bool DrawGUI(SerializedObject so)
    {
        //someGameObject = EditorGUILayout.ObjectField("GameObject", someGameObject, typeof(GameObject)) as GameObject;

        //so.Update();
        //
        //var prop = so.GetIterator();
        //prop.NextVisible(true);
        //while (prop.NextVisible(true)) {
        //    EditorGUILayout.PropertyField(prop);
        //}
        //
        //GUILayout.Space(5);
        //
        //if (GUI.changed) {
        //    so.ApplyModifiedProperties();
        //    return true;
        //}
        return false;
    }
}
