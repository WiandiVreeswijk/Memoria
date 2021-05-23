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

    public CursorLocker cursorLocker;

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
        Globals.GetMenuController().SetMenu("Pause");
        //pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        cursorLocker.UnlockMouse();
        gameIsPaused = true;
    }

    private void Resume() {
        Globals.GetMenuController().CloseMenu();
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        cursorLocker.LockMouse();
        gameIsPaused = false;
    }

    public void ResumeGame() {
        Resume();
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        cursorLocker.UnlockMouse();
        pauseMenuUI.SetActive(false);
        menuFadeAnimator.Play("FadeIn");
        mainMenuCamera.Priority = 12;

    }
}
