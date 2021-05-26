using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using DG.Tweening;

public class MainMenu : MonoBehaviour {
    public PlayableDirector director;
    public GameObject controlPanel;

    public CinemachineVirtualCamera mainMenuCamera;

    public CursorLocker cursorLocker;

    private void Start() {
        cursorLocker.UnlockMouse();
        director.stopped += Director_stopped;
    }

    private void OnDestroy() {
        director.stopped -= Director_stopped;
    }

    private void Director_stopped(PlayableDirector obj) {
        Globals.GetPlayer().movement.canMove = true;
        Globals.GetMenuController().NotifyPlayer("<font=\"Mouse SDF\"><size=36>u</size></font>  to move the camera");
        Utils.DelayedAction(10.0f, () => {
            if (!Globals.GetPlayer().movement.hasMoved) Globals.GetMenuController().NotifyPlayer("WASD to move");
        });
    }

    public void PlayGame() {
        cursorLocker.LockMouse();
        StartTimeLine();
        mainMenuCamera.Priority = 10;
        print("Play");
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void StartTimeLine() {
        director.time = 0;
        director.Evaluate();
        director.Play();
    }
}
