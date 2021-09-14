using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleManager : MonoBehaviour {
    private bool isPaused = true;
    public void PauseGame() {
        isPaused = true;
        Time.timeScale = 0.0f;
        Globals.CinemachineManager.SetPausedState(true);
    }

    public void UnPauseGame() {
        isPaused = false;
        Time.timeScale = 1.0f;
        Globals.CinemachineManager.SetPausedState(false);
    }

    public bool IsPaused() {
        return isPaused;
    }

    public void TogglePause() {
        if (isPaused) UnPauseGame();
        else PauseGame();
    }
}
