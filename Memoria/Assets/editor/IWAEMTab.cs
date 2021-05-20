using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IWAEMTab
{
    public void Initialize();
    public void OnGUI(EditorWindow window, GUIStyle style);
    public void OnUpdate();
}