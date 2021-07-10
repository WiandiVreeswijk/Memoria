using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour {
    public void OnPlayerDeath(Vector3 position, bool water) {
        Analytics.CustomEvent("Player Dies", new Dictionary<string, object> {
                {"Position", position},
                {"is Water", water}
            }); ;
    }
    public void OnCheckpointReached(int checkpointIndex) {
        Analytics.CustomEvent("Checkpoint reached", new Dictionary<string, object> {
                {"checkpoint reached", checkpointIndex},
            }); ;
    }

    public void OnLevelStarted() {

    }

    public void OnLevelEnded() {
        Analytics.CustomEvent("Level completed", new Dictionary<string, object> {
                {"Gems collected", Globals.ScoreManager.GetCollectibleCount()},
                {"Play duration level", Time.timeSinceLevelLoad}
            }); ;
    }
}
