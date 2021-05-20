using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class WAEMToolingWindow : EditorWindow {
    [MenuItem("WAEM/Main tooling")]
    public static void ShowWindow() {
        GetWindow<WAEMToolingWindow>("WAEM tooling");
    }

    enum Tab {
        Meshes,
        Baking,
        Testing,
        Building,
    }


    private int activeTab = 0;
    private string[] tabNames;
    private GUIStyle style;

    [SerializeField] private WAEMMeshesTab meshesTab;
    [SerializeField] private WAEMBakingTab bakingTab;
    [SerializeField] private WAEMTestingTab testingTab;
    [SerializeField] private WAEMBuildingTab buildingTab;


    
    private void OnEnable() {
        if (meshesTab == null) meshesTab = new WAEMMeshesTab();
        if (bakingTab == null) bakingTab = new WAEMBakingTab();
        if (testingTab == null) testingTab = new WAEMTestingTab();
        if (buildingTab == null) buildingTab = new WAEMBuildingTab();
    }

    private Dictionary<Tab, IWAEMTab> tabs;



    private void InitTabs() {
        tabNames = Enum.GetNames(typeof(Tab));

        tabs = new Dictionary<Tab, IWAEMTab>() {
            {Tab.Meshes, meshesTab},
            {Tab.Baking, bakingTab},
            {Tab.Testing, testingTab},
            {Tab.Building, buildingTab},
        };
        foreach (var tabPair in tabs) {
            tabPair.Value.Initialize();
        }
    }

    private void OnGUI() {
        if(tabs == null) InitTabs();
        style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        EditorGUILayout.Separator();
        activeTab = GUILayout.Toolbar(activeTab, tabNames);
        EditorGUILayout.LabelField($"<b>{tabNames[activeTab]}</b>", style);
        EditorGUILayout.Separator();
        tabs[(Tab)activeTab].OnGUI(this, style);
    }

    private void OnInspectorUpdate() {
        if (tabs == null) InitTabs();
        tabs[(Tab)activeTab].OnUpdate();
    }
}
