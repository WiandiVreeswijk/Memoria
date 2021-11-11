using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class ProgressionComponentBase : ScriptableObject {
    public abstract bool DrawGUI(SerializedObject so);
}