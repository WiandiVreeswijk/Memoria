using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OblivionManager : MonoBehaviour {
    private bool isInEditor = false;


    [Header("Game")]
    [Range(0.1f, 10.0f)] [SerializeField] private float oblivionSpeed = 1.0f;

    [Header("Position")]
    [Space(30.0f)]
    [SerializeField] private float oblivionPosition = 0.0f;
    [SerializeField] private float oblivionPositionOffset = 0.0f;

    [Header("Perlin")]
    [Range(0.0f, 20.0f)] [SerializeField] private float perlinAmount = 8.0f;
    [Range(0.0f, 10.0f)] [SerializeField] private float perlinScale = 5.0f;
    [Header("Voronoi")]
    [Range(0.0f, 10.0f)] [SerializeField] private float voronoiAmount = 4.75f;
    [Range(0.0f, 2.0f)] [SerializeField] private float voronoiSpeed = 1.0f;
    [Range(0.0f, 10.0f)] [SerializeField] private float voronoiDensity = 5.0f;
    [Range(0.0f, 0.05f)] [SerializeField] private float voronoiNormalStrength = 0.003f;

    [Header("Creep")]
    [Range(0.0f, 10.0f)] [SerializeField] private float creepMultiplier = 5.0f;
    [Range(0.0f, 10.0f)] [SerializeField] private float creepIntensity = 4.0f;

    [Header("Bumps")]
    [Range(0.0f, 200.0f)] [SerializeField] private float bumpsNormalStrength = 75.0f;
    [Range(0.0f, 10.0f)] [SerializeField] private float bumpsNoiseScale = 1.3f;

    [Header("Visual")]
    [ColorUsage(true, true)] [SerializeField] private Color oblivionColor = Color.black;
    [ColorUsage(true, true)] [SerializeField] private Color altColor = Color.white;
    [Range(0.0f, 1.0f)] [SerializeField] private float altColorIntensity = 0.5f;
    [Range(0.0f, 10.0f)] [SerializeField] private float altColorEdgeIntensity = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float smoothness = 0.7f;

    [Header("Debug")]
    [SerializeField] private bool debug = false;
    void Start() {
        isInEditor = !Application.isPlaying;
        Utils.EnsureOnlyOneInstanceInScene<OblivionManager>();

        if (!isInEditor) oblivionPosition = 0;
    }

    void Update() {
        if (isInEditor) EditorUpdate();
        else GameUpdate();
    }

    void EditorUpdate() {
        Shader.SetGlobalFloat("_OblivionPosition", -oblivionPosition);
        Shader.SetGlobalFloat("_OblivionPositionOffset", -oblivionPositionOffset);
        Shader.SetGlobalFloat("_PerlinScale", perlinScale);
        Shader.SetGlobalFloat("_PerlinAmount", perlinAmount);
        Shader.SetGlobalFloat("_VoronoiAmount", voronoiAmount);
        Shader.SetGlobalFloat("_VoronoiSpeed", voronoiSpeed);
        Shader.SetGlobalFloat("_VoronoiDensity", voronoiDensity);
        Shader.SetGlobalFloat("_VoronoiNormalStrength", voronoiNormalStrength);
        Shader.SetGlobalFloat("_CreepIntensity", creepIntensity);
        Shader.SetGlobalFloat("_CreepMultiplier", creepMultiplier);
        Shader.SetGlobalFloat("_AltColorIntensity", altColorIntensity);
        Shader.SetGlobalFloat("_BumpsNormalStrength", bumpsNormalStrength);
        Shader.SetGlobalFloat("_BumpsNoiseScale", bumpsNoiseScale);
        Shader.SetGlobalFloat("_AltColorEdgeIntensity", altColorEdgeIntensity);
        Shader.SetGlobalFloat("_Smoothness", smoothness);
        Shader.SetGlobalColor("_OblivionColor", oblivionColor);
        Shader.SetGlobalColor("_AltColor", altColor);

        if (debug) {
            Shader.SetGlobalFloat("_OblivionPositionOffset", 0);
            Shader.SetGlobalFloat("_PerlinAmount", 0);
            Shader.SetGlobalFloat("_VoronoiAmount", 0);
        }
    }

    void GameUpdate() {
        Shader.SetGlobalFloat("_OblivionPosition", -oblivionPosition);
        oblivionPosition += oblivionSpeed * Time.deltaTime;
    }

    public float GetOblivionPosition()
    {
        return oblivionPosition;
    }

    public void SetOblivionPosition(float position)
    {
        oblivionPosition = position;
    }
}
