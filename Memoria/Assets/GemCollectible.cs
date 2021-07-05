using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectible : MonoBehaviour {
    private ParticleSystem pSystem;

    public void Start() {
        Animator animator = GetComponentInChildren<Animator>();
        animator.Play("GemAnimation", 0, ((transform.position.x % 10) / 5.0f) - 1.0f);
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayPFX() {
        pSystem.Play();
    }
}