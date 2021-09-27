using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    public Transform lookAtPoint;
    private float lerp = 0.0f;
    private Tween tween;
    private bool toggle = false;
    public float weight = 1.0f;

    public float timeToLookAtPlayer = 2.0f;

    [Range(0.5f, 10f)]
    public float distance = 4.5f;
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float dist = Vector3.Distance(transform.position, Globals.Player.EyePosition);
        if (dist > distance) {
            //Stop
            if (!toggle) {
                tween?.Kill();
                tween = DOTween.To(() => lerp, x => lerp = x, 1.0f, timeToLookAtPlayer).SetEase(Ease.InOutQuad);
                toggle = true;
            }
        } else if (dist < (distance - 0.5f)) {
            //Start
            if (toggle) {
                tween?.Kill();
                tween = DOTween.To(() => lerp, x => lerp = x, 0.0f, timeToLookAtPlayer).SetEase(Ease.InOutQuad);
                toggle = false;
            }
        }
    }

    void OnAnimatorIK() {
        animator.SetLookAtPosition(Vector3.Lerp(Globals.Player.EyePosition, lookAtPoint.position, lerp));
        animator.SetLookAtWeight(weight);
    }
}
