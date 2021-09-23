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
        if (!hasJumpedOrBeenNotified) {
            Globals.UIManager.NotificationManager.NotifyPlayer("Space to jump");
            Destroy(gameObject);
            hasJumpedOrBeenNotified = true;
        }
    }

    public void OnLevelStarted() {
        movementTween = Utils.DelayedAction(5.0f, () => Globals.UIManager.NotificationManager.NotifyPlayer("WASD to move"));
    }
}
