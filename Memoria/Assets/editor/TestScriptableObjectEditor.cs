using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestScriptableObject), true)]
public class TestScriptableObjectEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save")) {
            TestScriptableObject obj = target as TestScriptableObject;
            obj.classes.Add(new TestScriptableObject.SomeClass());
            obj.classes.Add(new TestScriptableObject.SomeOtherClass());
            obj.classes.Add(new TestScriptableObject.AnotherClass());
            obj.Save();
        }

        if (GUILayout.Button("Clear")) {
            TestScriptableObject obj = target as TestScriptableObject;
            obj.classes.Clear();
            obj.Save();
        }
    }
}
