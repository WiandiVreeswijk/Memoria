using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class MainMenu : MonoBehaviour {
    public PlayableDirector director;
    public GameObject controlPanel;

    public CinemachineVirtualCamera mainMenuCamera;

    public CursorLocker cursorLocker;

    private void Start() {
        cursorLocker.UnlockMouse();
        director.stopped += Director_stopped;
    }

    private void Director_stopped(PlayableDirector obj) {
        Globals.GetMenuController().NotifyPlayer("WASD to move");
    }

    public void PlayGame() {
        cursorLocker.LockMouse();
        StartTimeLine();
        mainMenuCamera.Priority = 10;
    }

    private void Update() {
        if (Input.GetButton("Sprint")) PlayGame();
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void StartTimeLine() {
        director.Play();
    }
}
