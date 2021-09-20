using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OptionsMenu : MonoBehaviour {
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Animator menuFadeAnimator;
    public PlayableDirector director;
    public CinemachineVirtualCamera mainMenuCamera;

    private void Update() {
        if (director.state != PlayState.Playing) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (gameIsPaused) {
                    Resume();
                } else {
                    Pause();
                }
            }
        }
    }

    private void Pause() {
        Globals.MenuController.SetMenu("Pause");
        //pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Globals.CursorManager.UnlockMouse();
        gameIsPaused = true;
    }

    private void Resume() {
        Globals.MenuController.CloseMenu();
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Globals.CursorManager.LockMouse();
        gameIsPaused = false;
    }

    public void ResumeGame() {
        Resume();
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        Globals.CursorManager.UnlockMouse();
        pauseMenuUI.SetActive(false);
        menuFadeAnimator.Play("FadeIn");
        mainMenuCamera.Priority = 12;
    }
}
