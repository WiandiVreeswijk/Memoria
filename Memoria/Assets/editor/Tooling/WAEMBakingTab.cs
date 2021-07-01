using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class WAEMBakingTab : IWAEMTab {
    LightingSettings lowQualityLightingSettings = null;
    LightingSettings highQualityLightingSettings = null;
    private bool currentlyBaking = false;
    private bool assetsFound = false;

    public void Initialize() {
        FindSettingsAssets();
    }

    private void FindSettingsAssets() {
        foreach (var a in AssetDatabase.FindAssets("t:LightingSettings")) {
            string assetPath = AssetDatabase.GUIDToAssetPath(a);
            var asset = AssetDatabase.LoadAssetAtPath<LightingSettings>(assetPath);
            if (asset.name == "lowQualityLightingSettings") lowQualityLightingSettings = asset;
            if (asset.name == "highQualityLightingSettings") highQualityLightingSettings = asset;
        }

        assetsFound = lowQualityLightingSettings != null && highQualityLightingSettings != null;
        if (!assetsFound) {
            if (lowQualityLightingSettings == null) Debug.LogError("Failed to find lowQualityLightingSettings asset!");
            if (highQualityLightingSettings == null) Debug.LogError("Failed to find highQualityLightingSettings asset!");
        }
    }

    public void OnGUI(EditorWindow window, GUIStyle style) {
        if (!assetsFound) {
            GUILayout.Label("<color=red><size=16>Failed to find lighting settings assets!</size></color>", style);
            if (GUILayout.Button("Search again", GUILayout.Height(30))) FindSettingsAssets();
        } else {
            GUILayout.Label("<b>Occlusion baking</b>", style);
            if (GUILayout.Button("Bake occlusion culling", GUILayout.Height(30))) {
                StaticOcclusionCulling.Compute();
            }
            EditorGUILayout.Separator();

            GUILayout.Label("<b>Light baking</b>", style);
            if (GUILayout.Button("Bake low quality", GUILayout.Height(30))) {
                Lightmapping.lightingSettings = lowQualityLightingSettings;
                Lightmapping.BakeAsync();
            }
            if (GUILayout.Button("Bake high quality", GUILayout.Height(30))) {
                Lightmapping.lightingSettings = highQualityLightingSettings;
                Lightmapping.BakeAsync();
            }

            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!currentlyBaking);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cancel bake", GUILayout.Height(30), GUILayout.Width(100))) {
                Lightmapping.Cancel();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }
    }

    public void OnUpdate() {
        currentlyBaking = Lightmapping.isRunning;
    }

    public void OnSelectionChange(EditorWindow window) {

    }

    public void OnDestroy() {

    }
}
