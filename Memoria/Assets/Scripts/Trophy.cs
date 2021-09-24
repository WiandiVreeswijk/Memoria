using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour {
    [SerializeField] private TrophyType trophyType;

    public void SetMaterial(Material material) {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) {
            renderer.material = material;
        }
    }

    public TrophyType GetTrophyType() {
        return trophyType;
    }
}
