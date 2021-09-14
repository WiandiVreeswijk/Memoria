using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class WijkOpeningCutscene : MonoBehaviour {
    [Tooltip("Disable this to disable the opening cutscene.")]
    public bool isEnabled = true;
    public CinemachineVirtualCamera mainMenuCamera;
    public CinemachineVirtualCamera thirdPerson;
    public PlayableDirector playableDirector;

    public CarEngine busEngine;
    public BusStop busStop;

    void Start() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(false);
    }

    public void OnFinishCutscene() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(true);
        Utils.DelayedAction(10.0f, () => {
            if (!Globals.Player.PlayerMovementAdventure.HasMoved()) Globals.MenuController.NotifyPlayer("WASD to move");
        });
    }

    private bool ShouldSkip() {
        return !isEnabled && Application.isEditor;
    }

    private void MouseNotification() {
        Globals.MenuController.NotifyPlayer("<font=\"Mouse SDF\"><size=36>u</size></font>  to move the camera");
    }

    public void Trigger() {
        mainMenuCamera.Priority = 10;
        playableDirector.time = ShouldSkip() ? playableDirector.duration : 0;
        playableDirector.Evaluate();
        playableDirector.Play();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        busEngine.isBraking = false;
        if (ShouldSkip()) {
            busStop.Skip();
            MouseNotification();
            OnFinishCutscene();
        } else {
            Utils.DelayedAction(10.0f, () => MouseNotification());
        }
    }
}
