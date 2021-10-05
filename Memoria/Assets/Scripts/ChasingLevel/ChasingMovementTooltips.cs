using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChasingMovementTooltips : MonoBehaviour {
    private bool hasJumpedOrBeenNotified;
    private Tween movementTween;
        
    public void Update() {
        //#Todo generic jump input!
        if (Input.GetKeyDown(KeyCode.Space)) {
            hasJumpedOrBeenNotified = true;
        }
    }

    public void OnMovement() {
        movementTween?.Kill();
    }

    public void OnJumpPointReached() {
        if (!hasJumpedOrBeenNotified)
        {
            string localized = Globals.Localization.Get("INSTR_JUMP");
            Globals.UIManager.NotificationManager.NotifyPlayer(localized);
            Destroy(gameObject);
            hasJumpedOrBeenNotified = true;
        }
    }

    public void OnLevelStarted() {
        string localized = Globals.Localization.Get("INSTR_MOVE");
        movementTween = Utils.DelayedAction(5.0f, () => Globals.UIManager.NotificationManager.NotifyPlayer(localized));
    }
}
