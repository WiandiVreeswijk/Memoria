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
    public PlayableDirector playableDirector;

    public CarEngine busEngine;
    public BusStop busStop;
    private bool isStarted = false;

    void Start() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(false);
    }

    public void OnFinishCutscene() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(true);
        Utils.DelayedAction(7.5f, () => {
            if (!Globals.Player.PlayerMovementAdventure.HasMoved()) Globals.UIManager.NotificationManager.NotifyPlayer("WASD to move");
        });

        StartMarkerNotification();
    }

    private void StartMarkerNotification() {
        Tween tween = null;
        tween = Utils.DelayedAction(20.0f, () => {
            Globals.UIManager.NotificationManager.NotifyPlayer("Follow the ! marker");
        }).OnUpdate(() => {
            if (Vector3.Distance(Globals.Player.transform.position, Globals.ProgressionManager.GetIcon().transform.position) <
                3.5f) {
                print("Player got close to marker!");
                tween.Kill();
            }
        });
    }

    private bool ShouldSkip() {
        return !isEnabled && Application.isEditor;
    }

    private void MouseNotification() {
        Globals.UIManager.NotificationManager.NotifyPlayer("<font=\"Mouse SDF\"><size=78>u</size></font>  to move the camera");
    }

    public void Trigger() {
        if (isStarted) return;
        isStarted = true;
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
            Utils.DelayedAction(15.0f, () => MouseNotification());
        }
    }
}
