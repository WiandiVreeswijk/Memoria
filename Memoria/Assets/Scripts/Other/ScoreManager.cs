using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private int collectibles = 0;

    private void UpdateVisuals() {
        Globals.UIManager.ChasingLevel.SetCollectibleCount(collectibles);
    }

    public void ResetScore() {
        collectibles = 0;
        UpdateVisuals();
    }

    public void AddCollectible() {
        collectibles++;
        UpdateVisuals();
    }

    public int GetCollectibleCount() {
        return collectibles;
    }
}
