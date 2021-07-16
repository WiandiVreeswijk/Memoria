using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour {
    public void OnPlayerDeath(Vector3 position, bool water) {
        Analytics.CustomEvent("Player Dies", new Dictionary<string, object> {
                {"Position", position.x},
                {"is Water", water}
            }); ;
    }
    public void OnCheckpointReached(int checkpointIndex) {
        Analytics.CustomEvent("Checkpoint reached", new Dictionary<string, object> {
                {"checkpoint index", checkpointIndex + " Time (minutes)" + Time.timeSinceLevelLoad /60}
            }); ;
    }

    public void OnLevelStarted() {

    }

    public void OnLevelEnded() {
        Analytics.CustomEvent("Level completed", new Dictionary<string, object> {
                {"Gems collected", Globals.ScoreManager.GetCollectibleCount()},
                {"Play duration (minutes)", Time.timeSinceLevelLoad /60}
            }); ;
    }
}
