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
    Tween markerNotificationTween;

    void Start() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(false);
        Globals.Player.VisualEffects.SetVisible(false);
    }

    public void OnFinishCutscene() {
        Globals.Player.PlayerMovementAdventure.SetCanMove(true);
        Utils.DelayedAction(7.5f, () => {
            if (!Globals.Player.PlayerMovementAdventure.HasMoved()) Globals.UIManager.NotificationManager.NotifyPlayer("WASD to move");
        });
        StartMarkerNotification();
    }

    public void KillMarkerNotificationTween() {
        markerNotificationTween.Kill();
        print("Marker notification killed!");
    }

    private void StartMarkerNotification() {
        markerNotificationTween = Utils.DelayedAction(25.0f, () => {
            Globals.UIManager.NotificationManager.NotifyPlayer("Follow the ! marker");
            markerNotificationTween.Restart();
        }).OnUpdate(() => {
            if (Globals.GetCurrentGlobalsType() != Globals.GlobalsType.NEIGHBORHOOD) markerNotificationTween.Kill();
            if (Vector3.Distance(Globals.Player.transform.position, Globals.ProgressionManager.GetIcon().transform.position) < 3.5f) {
                print("Player got close to marker!");
                markerNotificationTween.Restart();
            }
        });
    }

    private bool ShouldSkip() {
        return !isEnabled && Application.isEditor;
    }

    private void MouseNotification() {
        Globals.UIManager.NotificationManager.NotifyPlayer("<font=\"Mouse SDF\"><size=78>u</size></font>  to move the camera");
    }

    public void OnBusArrive() {
        Globals.Player.VisualEffects.SetVisible(true);
    }

    public void Trigger() {
        if (isStarted) return;
        isStarted = true;
        mainMenuCamera.Priority = 10;
        playableDirector.time = ShouldSkip() ? playableDirector.duration : 0;
        playableDirector.Evaluate();
        playableDirector.Play();

        busEngine.isBraking = false;
        if (ShouldSkip()) {
            busStop.Skip();
            MouseNotification();
            Globals.Player.VisualEffects.SetVisible(true);
            OnFinishCutscene();
        } else {
            Utils.DelayedAction(15.0f, () => MouseNotification());
        }
    }
}
