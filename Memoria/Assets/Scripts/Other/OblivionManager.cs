using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OblivionManager : MonoBehaviour {
    [Header("Position")]
    [SerializeField] private float oblivionPosition = 0.0f;
    [SerializeField] private float oblivionPositionOffset = 0.0f;

    [Header("Perlin")]
    [Range(0.0f, 10.0f)] [SerializeField] private float perlinAmount = 8.0f;
    [Range(0.0f, 10.0f)] [SerializeField] private float perlinScale = 5.0f;
    [Header("Voronoi")]
    [Range(0.0f, 10.0f)] [SerializeField] private float voronoiAmount = 4.75f;
    [Range(0.0f, 2.0f)] [SerializeField] private float voronoiSpeed = 1.0f;
    [Range(0.0f, 10.0f)] [SerializeField] private float voronoiDensity = 5.0f;

    [Header("Color")]
    [ColorUsage(true, true)] [SerializeField] private Color oblivionColor = Color.black;

    [Header("Debug")]
    [SerializeField] private bool debug = false;
    void Start() {
        Utils.EnsureOnlyOneInstanceInScene<OblivionManager>();
    }

    void Update() {
        if (debug) {
            Shader.SetGlobalFloat("_OblivionPosition", -oblivionPosition);
            Shader.SetGlobalFloat("_OblivionPositionOffset", -oblivionPositionOffset);
            Shader.SetGlobalFloat("_PerlinAmount", 0);
            Shader.SetGlobalFloat("_VoronoiAmount", 0);
            Shader.SetGlobalColor("_OblivionColor", oblivionColor);
        } else {
            Shader.SetGlobalFloat("_OblivionPosition", -oblivionPosition);
            Shader.SetGlobalFloat("_OblivionPositionOffset", -oblivionPositionOffset);
            Shader.SetGlobalFloat("_PerlinScale", perlinScale);
            Shader.SetGlobalFloat("_PerlinAmount", perlinAmount);
            Shader.SetGlobalFloat("_VoronoiAmount", voronoiAmount);
            Shader.SetGlobalFloat("_VoronoiSpeed", voronoiSpeed);
            Shader.SetGlobalFloat("_VoronoiDensity", voronoiDensity);
            Shader.SetGlobalColor("_OblivionColor", oblivionColor);
        }
    }
}
