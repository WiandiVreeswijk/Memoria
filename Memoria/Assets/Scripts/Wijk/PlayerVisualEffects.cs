using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    private List<Material> playerMaterials;
    private List<Renderer> skinnedRenderers;

    void Start()
    {
        skinnedRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        playerMaterials = new List<Material>();

        foreach (var renderer in skinnedRenderers) playerMaterials.Add(renderer.material);
    }

    public void SetMeshEnabled(bool enabled) {
        skinnedRenderers.ForEach(x => x.enabled = enabled);
    }
}
