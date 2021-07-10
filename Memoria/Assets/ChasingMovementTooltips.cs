using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChasingMovementTooltips : MonoBehaviour {
    private bool hasJumpedOrBeenNotified;
    private Tween movementTween;
    void Start() {
        movementTween = Utils.DelayedAction(5.0f, () => Globals.MenuController.NotifyPlayer("WASD to move"));
    }

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
            Globals.MenuController.NotifyPlayer("Space to jump");
            Destroy(gameObject);
            hasJumpedOrBeenNotified = true;
        }
    }
}
