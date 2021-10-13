using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class WijkOpeningCutscene : MonoBehaviour {
    [Tooltip("Disable this to disable the opening cutscene.")]
    [SerializeField] private bool isEnabled = true;
    public CinemachineFreeLook thirdPersonCamera;
    public PlayableDirector playableDirector;

    public CarEngine busEngine;
    public BusStop busStop;
    private bool isStarted = false;
    Tween markerNotificationTween;

    private bool skipped = false;

    public void SetEnabled(bool toggle) {
        isEnabled = toggle;
    }

    void Start() {
        if (!skipped) {
            Globals.Player.PlayerMovementAdventure.SetCanMove(false);
            Globals.Player.VisualEffects.SetVisible(false);
        }
    }

    public void OnFinishCutscene() {
        Globals.CinemachineManager.SetInputEnabled(true);
        Globals.Player.PlayerMovementAdventure.SetCanMove(true);
        StartMovementNotification(8f);
    }

    public void StartMovementNotification(float delay) {
        Utils.DelayedAction(delay, () => {
            string localized = Globals.Localization.Get("INSTR_MOVE");
            if (!Globals.Player.PlayerMovementAdventure.HasMoved()) {
                Globals.UIManager.NotificationManager.NotifyPlayer(localized);
                StartMovementNotification(15f);
            } else StartMarkerNotification();
        });
    }

    public void KillMarkerNotificationTween() {
        markerNotificationTween.Kill();
        print("Marker notification killed!");
    }

    private void StartMarkerNotification() {
        markerNotificationTween = Utils.DelayedAction(25.0f, () => {
            string localized = Globals.Localization.Get("INSTR_MARKER");
            Globals.UIManager.NotificationManager.NotifyPlayer(localized);
            markerNotificationTween.Restart();
        }).OnUpdate(() => {
            if (Globals.GetCurrentGlobalsType() != Globals.GlobalsType.NEIGHBORHOOD) markerNotificationTween.Kill();
            if (Vector3.Distance(Globals.Player.transform.position, Globals.ProgressionManager.GetIcon().transform.position) < 3.5f) {
                print("Player got close to marker!");
                markerNotificationTween.Restart();
            }
        });
    }

    public bool ShouldSkip() {
        return !isEnabled && Application.isEditor;
    }

    private void MouseNotification() {
        string localized = Globals.Localization.Get("INSTR_CAMERA");
        Globals.UIManager.NotificationManager.NotifyPlayer("<font=\"Mouse SDF\"><size=78>u</size></font> " + localized);
    }

    public void OnBusArrive() {
        Globals.ProgressionManager.GetIcon().SetEnabled(true);
        Globals.Player.VisualEffects.SetVisible(true);
        Globals.CinemachineManager.SetInputEnabled(false);
        thirdPersonCamera.m_Transitions.m_InheritPosition = true;
        Utils.DelayedAction(15.0f, () => {
            thirdPersonCamera.m_Transitions.m_InheritPosition = false;
        });

        foreach (DialogueIcon dialogueIcon in FindObjectsOfType<DialogueIcon>()) {
            dialogueIcon.SetEnabled(true);
        }
    }

    public void Trigger() {
        if (isStarted) return;
        isStarted = true;
        playableDirector.time = ShouldSkip() ? playableDirector.duration : 0;
        playableDirector.Evaluate();
        playableDirector.Play();

        busEngine.isBraking = false;
        if (ShouldSkip()) {
            Skip();
        } else {
            Utils.DelayedAction(15.0f, () => MouseNotification());
        }
    }

    public void Skip() {
        skipped = true;
        busStop.Skip();
        MouseNotification();
        Globals.Player.VisualEffects.SetVisible(true);
        OnFinishCutscene();
    }
}
