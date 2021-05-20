using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WAEMToolingWindow : EditorWindow {
    [MenuItem("WAEM/Main tooling")]
    public static void ShowWindow() {
        var window = GetWindow<WAEMToolingWindow>("WAEM tooling");
    }

    private int activeTab = 0;
    private string[] tabNames;
    private GUIStyle style;

    private void OnEnable() {
        tabNames = Enum.GetNames(typeof(Tab));
        foreach (var tabPair in tabs) {
            tabPair.Value.Initialize();
        }
    }

    enum Tab {
        Meshes,
        Baking,
        Testing,
    }

    private Dictionary<Tab, WAEMTab> tabs = new Dictionary<Tab, WAEMTab>()
    {
        {Tab.Meshes, new WAEMMeshesTab()},
        {Tab.Baking, new WAEMBakingTab()},
        {Tab.Testing, new WAEMTestingTab()},
    };

    private void OnGUI() {
        style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        EditorGUILayout.Separator();
        activeTab = GUILayout.Toolbar(activeTab, tabNames);
        EditorGUILayout.LabelField($"<b>{tabNames[activeTab]}</b>", style);
        EditorGUILayout.Separator();
        tabs[(Tab)activeTab].OnGUI(style);
    }

    private void OnInspectorUpdate() {
        tabs[(Tab)activeTab].OnUpdate();
    }
}
