using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectible : MonoBehaviour {
    private ParticleSystem pSystem;

    public void Start() {
        Animator animator = GetComponentInChildren<Animator>();
        float val = ((transform.position.x % 5) / 2.5f) - 1.0f;
        animator.Play("GemAnimation", 0, val);
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayPFX() {
        pSystem.Play();
    }
}