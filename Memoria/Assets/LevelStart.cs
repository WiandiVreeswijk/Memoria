using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour {
    public GameObject timelinePlayer;
    public void OnLevelStarted() {
        FindObjectOfType<ChasingMovementTooltips>().OnLevelStarted();
        Globals.Player.transform.position = timelinePlayer.transform.position;
        Globals.AnalyticsManager.OnLevelStarted();
    }
}
