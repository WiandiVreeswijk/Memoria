#if UNITY_EDITOR
#pragma warning disable 0618
#pragma warning disable 0612
using UnityEngine;
using System;


namespace UnityEditor
{
    public class NL_Bakery_SH_GUI : ShaderGUI
    {
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
            public static string fresnelHeaderText = "Fresnel Modifiers";
            public static GUIContent subFresnelScaleText = new GUIContent("Subtract Fresnel Scale", "The width of the subtracted Fresnel effect. This helps to reduce very thin outline flickering, when the object seen from distance.");
            public static GUIContent subFresnelPowerText = new GUIContent("Subtract Fresnel Power", "The power of subtact Fresnel effect. This helps to reduce very thin outline flickering, when the object seen from distance.");
        }

        MaterialProperty albedoMap = null;
        MaterialProperty albedoColor = null;
        MaterialProperty metallicMap = null;
        MaterialProperty metallic = null;
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
        MaterialProperty subFresnelScale = null;
        MaterialProperty subFresnelPower = null;

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
            subFresnelScale = FindProperty("_SubtractFresnelScale", props);
            subFresnelPower = FindProperty("_SubtractFresnelPower", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

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

                
                GUILayout.Label(Styles.fresnelHeaderText, EditorStyles.boldLabel);

                m_MaterialEditor.ShaderProperty(subFresnelScale, Styles.subFresnelScaleText);
                if (subFresnelScale.floatValue > 0)
                {
                    m_MaterialEditor.ShaderProperty(subFresnelPower, Styles.subFresnelPowerText);
                }
            }
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

            // Emission for GI?
            m_MaterialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
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