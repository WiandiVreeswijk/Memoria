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

    void Start() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(false);
    }

    public void OnFinishCutscene() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(true);
        Globals.MenuController.NotifyPlayer("<font=\"Mouse SDF\"><size=36>u</size></font>  to move the camera");
        Utils.DelayedAction(45.0f, () => {
            if (!Globals.Player.PlayerMovementAdventure.HasMoved()) Globals.MenuController.NotifyPlayer("WASD to move");
        });
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    private bool ShouldSkip()
    {
        return !isEnabled && Application.isEditor;
    }

    public void Trigger() {
        mainMenuCamera.Priority = 10;
        playableDirector.time = ShouldSkip() ? playableDirector.duration : 0;
        playableDirector.Evaluate();
        playableDirector.Play();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        busEngine.isBraking = false;
        playableDirector.Evaluate();
        if (ShouldSkip()) {
            OnFinishCutscene();
        }
        //cursorLocker.LockMouse();
    }
}
