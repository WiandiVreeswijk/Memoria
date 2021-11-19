using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenuAttribute]
public class TestScriptableObject : StyledScriptableObject {


    [Serializable]
    public class SomeClass {
        public int data = 0;
    }

    [Serializable]
    public class SomeOtherClass : SomeClass {
        public string test = "test";
    }

    [Serializable]
    public class AnotherClass : SomeClass {
        public float value = 0.4543f;
        public ProgressionSceneNodeReference reference;
        public AnotherClass() {
        }
    }

    [SerializeReference] public List<SomeClass> classes = new List<SomeClass>();

    public void Save() {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }
}