using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour {
    private Animator animator;
    private Renderer[] meshRenderers;

    void Start() {
        meshRenderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
    }

    public void SetVisible(bool toggle) {
        foreach (Renderer renderer in meshRenderers) {
            renderer.enabled = toggle;
        }
    }

    public void SetLookAt(Vector3? position) {
        if (position.HasValue) {
            print(position.Value);
            GetComponent<Animator>().SetLookAtPosition(position.Value);
            GetComponent<Animator>().SetLookAtWeight(1f);
        } else {
            GetComponent<Animator>().SetLookAtWeight(0f);
        }
    }
}
