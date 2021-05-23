using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LookAtTest : MonoBehaviour {
    public Transform lookAtPoint;
    public Transform PlayerLookAtPoint;
    private float lerp = 0.0f;
    private Tween tween;
    private bool toggle = false;
    Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float dist = Vector3.Distance(transform.position, PlayerLookAtPoint.position);
        if (dist > 4.5f) {
            //Stop
            if (!toggle) {
                if (tween != null) tween.Kill();
                tween = DOTween.To(() => lerp, x => lerp = x, 1.0f, 2.0f).SetEase(Ease.InOutQuad);
                toggle = true;
            }
        } else if (dist < 4.0f) {
            //Start
            if (toggle) {
                if (tween != null) tween.Kill();
                tween = DOTween.To(() => lerp, x => lerp = x, 0.0f, 2.0f).SetEase(Ease.InOutQuad);
                toggle = false;
            }
        }
    }

    void OnAnimatorIK() {
        animator.SetLookAtPosition(Vector3.Lerp(PlayerLookAtPoint.position, lookAtPoint.position, lerp));
        animator.SetLookAtWeight(1.0f);
    }
}
