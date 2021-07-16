using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class WijkOpeningCutscene : MonoBehaviour {
    [Tooltip("Disable this to disable the opening cutscene.")]
    public bool isEnabled = true;
    public CinemachineVirtualCamera mainMenuCamera;
    public CinemachineVirtualCamera thirdPerson;
    public PlayableDirector playableDirector;

    void Start() {
        if (!isEnabled && Application.isEditor) {
            GetComponentInChildren<PlayableDirector>().enabled = false;
        }
    }

    public void OnFinishCutscene() {
        Globals.Player.PlayerMovementAdventure.canMove = true;
        Globals.MenuController.NotifyPlayer("<font=\"Mouse SDF\"><size=36>u</size></font>  to move the camera");
        Utils.DelayedAction(10.0f, () => {
            if (!Globals.Player.PlayerMovementAdventure.hasMoved) Globals.MenuController.NotifyPlayer("WASD to move");
        });
    }

    public void Trigger() {
        mainMenuCamera.Priority = 10;
        playableDirector.time = 0;
        playableDirector.Evaluate();
        playableDirector.Play();
        if (!isEnabled && Application.isEditor) {
            OnFinishCutscene();
        }
        //cursorLocker.LockMouse();
    }
}
