using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour {
    public void OnLevelStarted() {
        Globals.AnalyticsManager.OnLevelStarted();
    }
}
