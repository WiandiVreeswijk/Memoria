using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour {
    private int count = 0;
    public void OnLevelStarted() {
        count++;
        if (count > 1) {
            Globals.AnalyticsManager.OnLevelStarted();
        }
    }
}
