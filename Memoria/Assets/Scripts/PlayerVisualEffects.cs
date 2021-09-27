using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour {
    private Animator animator;
    private Renderer[] meshRenderers;
    private Vector3 lookAtPoint;
    private float lookAtWeight;

    void Start() {
        meshRenderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
    }

    public void SetVisible(bool toggle) {
        foreach (Renderer renderer in meshRenderers) {
            renderer.enabled = toggle;
        }
    }

    Tween lookAtTween;
    public void SetLookAt(Vector3? position, float weight = 1.0f) {
        lookAtTween?.Kill();
        if (position.HasValue) {
            lookAtTween = DOTween.To(() => lookAtWeight, x => lookAtWeight = x, weight, 2.0f);
            lookAtPoint = position.Value;
        } else {
            lookAtTween = DOTween.To(() => lookAtWeight, x => lookAtWeight = x, 0, 2.0f);
        }
    }

    void OnAnimatorIK() {
        animator.SetLookAtPosition(lookAtPoint);
        animator.SetLookAtWeight(lookAtWeight);
    }
}
