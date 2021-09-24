using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour {
    private Animator animator;
    private Renderer[] meshRenderers;
    private bool shouldLookAt = false;
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

    public void SetLookAt(Vector3? position, float weight = 1.0f) {
        shouldLookAt = position.HasValue;
        if (shouldLookAt) {
            lookAtPoint = position.Value;
            lookAtWeight = weight;
            shouldLookAt = true;
        }
    }

    void OnAnimatorIK() {
        if (shouldLookAt) {
            animator.SetLookAtPosition(lookAtPoint);
            animator.SetLookAtWeight(lookAtWeight);
        } else {
            animator.SetLookAtWeight(0.0f);
        }
    }
}
