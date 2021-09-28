#if UNITY_EDITOR
#pragma warning disable 0618
#pragma warning disable 0612
using UnityEngine;
using System;


namespace UnityEditor
{
    public class NL_Bakery_Dynamic_SH_GUI : ShaderGUI
    {
        MaterialPropertyBlock mb;
        static MaterialPropertyBlock mbEmpty; // default empty block, no values (will revert to global volume)
        static int mVolumeMin, mVolumeInvSize;

        public enum SmoothnessMapChannel
        {
            SpecularMetallicAlpha,
            AlbedoAlpha,
        }

        private static class Styles
        {
            public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Smoothness (A)");
            public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R), Occlusion (G), Smoothness (A)");
            public static GUIContent smoothnessText = new GUIContent("Smoothness", "Smoothness value");
            public static GUIContent smoothnessScaleText = new GUIContent("Smoothness", "Smoothness scale factor");
            public static GUIContent smoothnessMapChannelText = new GUIContent("Source", "Smoothness texture and channel");
            public static GUIContent occlusionMapChannelText = new GUIContent("Source", "Occlusion map channel");
            public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map");
            public static GUIContent occlusionText = new GUIContent("Occlusion", "Occlusion (G)");
            public static GUIContent emissionText = new GUIContent("Emission", "Emission (RGB)");
            public static GUIContent emissionUseMapText = new GUIContent("Use Emission Map", "Use RGB map for emission");
            public static string bakeryText = "Bakery Options";
            public static GUIContent volLabel0 = new GUIContent("Volume 0");
            public static GUIContent volLabel1 = new GUIContent("Volume 1");
            public static GUIContent volLabel2 = new GUIContent("Volume 2");
            public static string shadowsBoostHeaderText = "Shadows Boost";
            public static GUIContent shadowsBoostThresholdText = new GUIContent("Threshold", "A threshold value to start boost shadows from. Higher values give sharper transition between affected and non affected areas.");
            public static GUIContent shadowBoostText = new GUIContent("Shadow Boost", "Amount of shadow boost. The more this value is, the more brighter shadows become.");
            public static string fresnelHeaderText = "Fresnel and Specular Modifiers";
            public static GUIContent specMultiplierText = new GUIContent("Specular Multiplier", "Amount of specular. Value of 1 - physically correct. This multiplier is helpful to compensate the highlights after the Subtract Fresnel effect.");
            public static GUIContent fresnelPowerText = new GUIContent("Fresnel Power", "The power of Fresnel effect. The more the value, the less specular seen from perpendicular to surface view. Value of 3 is good.");
            public static GUIContent subFresnelScaleText = new GUIContent("Subtract Fresnel Scale", "The width of the subtracted Fresnel effect. This helps to reduce very thin outline flickering, when the object seen from distance.");
            public static GUIContent subFresnelPowerText = new GUIContent("Subtract Fresnel Power", "The power of subtact Fresnel effect. This helps to reduce very thin outline flickering, when the object seen from distance.");
        }

        MaterialProperty albedoMap = null;
        MaterialProperty albedoColor = null;
        MaterialProperty metallicMap = null;
        MaterialProperty metallic = null;
        //MaterialProperty smoothness = null;
        MaterialProperty smoothnessScale = null;
        MaterialProperty smoothnessMapChannel = null;
        MaterialProperty occlusionMapChannel = null;
        MaterialProperty bumpScale = null;
        MaterialProperty bumpMap = null;
        MaterialProperty occlusionStrength = null;
        MaterialProperty occlusionMap = null;
        MaterialProperty emissionColorForRendering = null;
        MaterialProperty emissionMap = null;
        MaterialProperty useEmissionMap = null;
        MaterialProperty volume0 = null;
        MaterialProperty volume1 = null;
        MaterialProperty volume2 = null;
        MaterialProperty volumeMin = null;
        MaterialProperty volumeInvSize = null;
        MaterialProperty shadowBoostThreshold = null;
        MaterialProperty shadowBoost = null;
        MaterialProperty specMultiplier = null;
        MaterialProperty fresnelPower = null;
        MaterialProperty subFresnelScale = null;
        MaterialProperty subFresnelPower = null;

        BakeryVolume assignedVolume = null;

        MaterialEditor m_MaterialEditor;

        ColorPickerHDRConfig m_ColorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

        bool m_FirstTimeApply = true;

        public void FindProperties(MaterialProperty[] props)
        {
            albedoMap = FindProperty("_BaseMap", props);
            albedoColor = FindProperty("_BaseColor", props);
            metallicMap = FindProperty("_MetallicGlossMap", props, false);
            smoothnessScale = FindProperty("_Smoothness", props, false);
            smoothnessMapChannel = FindProperty("_SmoothnessTextureChannel", props, false);
            occlusionMapChannel = FindProperty("_AOSource", props, false);
            bumpScale = FindProperty("_BumpScale", props);
            bumpMap = FindProperty("_BumpMap", props);
            occlusionStrength = FindProperty("_OcclusionStrength", props);
            occlusionMap = FindProperty("_OcclusionMap", props);
            emissionColorForRendering = FindProperty("_EmissionColor", props);
            emissionMap = FindProperty("_EmissionMap", props);
            useEmissionMap = FindProperty("_UseEmissionMap", props);
            volume0 = FindProperty("_Volume0", props);
            volume1 = FindProperty("_Volume1", props);
            volume2 = FindProperty("_Volume2", props);
            volumeMin = FindProperty("_VolumeMin", props);
            volumeInvSize = FindProperty("_VolumeInvSize", props);
            shadowBoostThreshold = FindProperty("_BoostThreshold", props);
            shadowBoost = FindProperty("_ShadowBoost", props);
            specMultiplier = FindProperty("_Specpower", props);
            fresnelPower = FindProperty("_Fresnelpower", props);
            subFresnelScale = FindProperty("_SubtractFresnelScale", props);
            subFresnelPower = FindProperty("_SubtractFresnelPower", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            // Make sure that needed keywords are set up if we're switching some existing
            // material to a standard shader.
            if (m_FirstTimeApply)
            {
                SetMaterialKeywords(material);
                m_FirstTimeApply = false;
            }

            ShaderPropertiesGUI(material);
        }
        
        public void ShaderPropertiesGUI(Material material)
        {
            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;
            
            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            {
                // Primary properties
                DoAlbedoArea(material);
                DoSpecularMetallicArea();
                m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
                DoOcclusionArea();

                DoEmissionArea(material);

                EditorGUI.BeginChangeCheck();
                m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);

                if (EditorGUI.EndChangeCheck())
                {
                    emissionMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset;
                    bumpMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset;
                    metallicMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset;
                    occlusionMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset;
                }
                EditorGUILayout.Space();

                GUILayout.Label(Styles.bakeryText, EditorStyles.boldLabel);

                //volumes
                var prevAssignedVolume = assignedVolume;
                assignedVolume = EditorGUILayout.ObjectField(volume0.textureValue == null ? "Assign volume" : "Assign different volume", assignedVolume, typeof(BakeryVolume), true) as BakeryVolume;
                if (prevAssignedVolume != assignedVolume)
                {
                    volume0.textureValue = assignedVolume.bakedTexture0;
                    volume1.textureValue = assignedVolume.bakedTexture1;
                    volume2.textureValue = assignedVolume.bakedTexture2;
                    var b = assignedVolume.bounds;
                    volumeMin.vectorValue = b.min;
                    volumeInvSize.vectorValue = new Vector3(1.0f / b.size.x, 1.0f / b.size.y, 1.0f / b.size.z);
                    assignedVolume = null;
                }
                if (volume0.textureValue != null)
                {
                    if (GUILayout.Button("Unset volume"))
                    {
                        volume0.textureValue = null;
                        volume1.textureValue = null;
                        volume2.textureValue = null;
                        volumeMin.vectorValue = Vector3.zero;
                        volumeInvSize.vectorValue = Vector3.zero;
                    }
                }
                EditorGUILayout.LabelField("Current Volume: " + (volume0.textureValue == null ? "<none or global>" : volume0.textureValue.name.Substring(0, volume0.textureValue.name.Length - 1)));
                EditorGUI.BeginDisabledGroup(true);
                m_MaterialEditor.TexturePropertySingleLine(Styles.volLabel0, volume0);
                m_MaterialEditor.TexturePropertySingleLine(Styles.volLabel1, volume1);
                m_MaterialEditor.TexturePropertySingleLine(Styles.volLabel2, volume2);
                var bmin4 = volumeMin.vectorValue;
                var bmin = new Vector3(bmin4.x, bmin4.y, bmin4.z);
                var invSize = volumeInvSize.vectorValue;
                var bmax = new Vector3(1.0f / invSize.x + bmin.x, 1.0f / invSize.y + bmin.y, 1.0f / invSize.z + bmin.z);
                EditorGUILayout.LabelField("Min: " + bmin);
                EditorGUILayout.LabelField("Max: " + bmax);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space();

                GUILayout.Label(Styles.shadowsBoostHeaderText, EditorStyles.boldLabel);

                m_MaterialEditor.ShaderProperty(shadowBoostThreshold, Styles.shadowsBoostThresholdText);
                if (shadowBoostThreshold.floatValue > 0) {
                    m_MaterialEditor.ShaderProperty(shadowBoost, Styles.shadowBoostText);
                }

                EditorGUILayout.Space();

                GUILayout.Label(Styles.fresnelHeaderText, EditorStyles.boldLabel);

                m_MaterialEditor.ShaderProperty(fresnelPower, Styles.fresnelPowerText);

                m_MaterialEditor.ShaderProperty(subFresnelScale, Styles.subFresnelScaleText);
                if (subFresnelScale.floatValue > 0)
                {
                    m_MaterialEditor.ShaderProperty(subFresnelPower, Styles.subFresnelPowerText);
                }
                m_MaterialEditor.ShaderProperty(specMultiplier, Styles.specMultiplierText);
            }
        }

        static void SetMaterialKeywords(Material material)
        {

        }

        void DoEmissionArea(Material material)
        {
            bool hadEmissionTexture = emissionMap.textureValue != null;

            // Texture and HDR color controls
            m_MaterialEditor.TexturePropertyWithHDRColor(Styles.emissionText, emissionMap, emissionColorForRendering, m_ColorPickerHDRConfig, false);

            int indentation = 2;
            m_MaterialEditor.ShaderProperty(useEmissionMap, Styles.emissionUseMapText, indentation);

            // If texture was assigned and color was black set color to white
            float brightness = emissionColorForRendering.colorValue.maxColorComponent;
            if (emissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
                emissionColorForRendering.colorValue = Color.white; 
        }

        void DoAlbedoArea(Material material)
        {
            m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap);
            m_MaterialEditor.ColorProperty(albedoColor, "          Base Color");
        }

        void DoSpecularMetallicArea()
        {
            bool hasGlossMap = false;

                hasGlossMap = metallicMap.textureValue != null;
                m_MaterialEditor.TexturePropertySingleLine(Styles.metallicMapText, metallicMap, hasGlossMap ? null : metallic);

            bool showSmoothnessScale = hasGlossMap;
            if (smoothnessMapChannel != null)
            {
                int smoothnessChannel = (int)smoothnessMapChannel.floatValue;
                if (smoothnessChannel == (int)SmoothnessMapChannel.AlbedoAlpha)
                    showSmoothnessScale = true;
            }

            int indentation = 2; // align with labels of texture properties
            m_MaterialEditor.ShaderProperty(showSmoothnessScale ? smoothnessScale : smoothnessScale, showSmoothnessScale ? Styles.smoothnessScaleText : Styles.smoothnessText, indentation);

            ++indentation;
            if (smoothnessMapChannel != null)
                m_MaterialEditor.ShaderProperty(smoothnessMapChannel, Styles.smoothnessMapChannelText, indentation);
        }

        void DoOcclusionArea()
        {
            bool hasOcclusionMap = false;

            hasOcclusionMap = occlusionMap.textureValue != null;
            m_MaterialEditor.TexturePropertySingleLine(Styles.occlusionText, occlusionMap, occlusionStrength);

            if (occlusionMapChannel != null)
            {
                int occlusionChannel = (int)occlusionMapChannel.floatValue;

                int indentation = 2;

                ++indentation;
                m_MaterialEditor.ShaderProperty(occlusionMapChannel, Styles.occlusionMapChannelText, indentation);
            }
        }
    }
}
#endif