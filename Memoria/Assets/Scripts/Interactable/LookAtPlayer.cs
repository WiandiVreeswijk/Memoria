using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    public Transform lookAtPoint;
    private float lerp = 0.0f;
    private Tween tween;
    private bool toggle = false;
    Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float dist = Vector3.Distance(transform.position, Globals.Player.HeadPosition);
        if (dist > 4.5f) {
            //Stop
            if (!toggle) {
                tween?.Kill();
                tween = DOTween.To(() => lerp, x => lerp = x, 1.0f, 2.0f).SetEase(Ease.InOutQuad);
                toggle = true;
            }
        } else if (dist < 4.0f) {
            //Start
            if (toggle) {
                tween?.Kill();
                tween = DOTween.To(() => lerp, x => lerp = x, 0.0f, 2.0f).SetEase(Ease.InOutQuad);
                toggle = false;
            }
        }
    }

    void OnAnimatorIK() {
        animator.SetLookAtPosition(Vector3.Lerp(Globals.Player.HeadPosition, lookAtPoint.position, lerp));
        animator.SetLookAtWeight(1.0f);
    }
}
