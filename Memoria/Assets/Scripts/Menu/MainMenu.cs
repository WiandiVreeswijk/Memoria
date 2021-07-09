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
        //#Todo This is extremely dirty and will break very soon
        director = FindObjectOfType<PlayableDirector>();
        //cursorLocker.UnlockMouse();
        director.stopped += Director_stopped;
    }

    private void OnDestroy() {
        director.stopped -= Director_stopped;
    }

    private void Director_stopped(PlayableDirector obj) {
        Globals.Player.PlayerMovementAdventure.canMove = true;
        Globals.MenuController.NotifyPlayer("<font=\"Mouse SDF\"><size=36>u</size></font>  to move the camera");
        Utils.DelayedAction(10.0f, () => {
            if (!Globals.Player.PlayerMovementAdventure.hasMoved) Globals.MenuController.NotifyPlayer("WASD to move");
        });
    }

    public void PlayGame() {
        FindObjectOfType<WijkOpeningCutscene>().Trigger();
        Globals.MenuController.CloseMenu();
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
