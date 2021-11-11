using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProgressionComponent : ProgressionComponentBase {
    public GameObject someGameObject;

    public override bool DrawGUI(SerializedObject so) {
        so.Update();

        var prop = so.GetIterator();
        prop.NextVisible(true);
        while (prop.NextVisible(true)) {
            EditorGUILayout.PropertyField(prop);
        }

        GUILayout.Space(5);

        if (GUI.changed) {
            so.ApplyModifiedProperties();
            return true;
        }
        return false;
    }
}
