using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCollectible : MonoBehaviour {
    public float rotationSpeed = 2f;

    public ParticleSystem hoveringParticles, collectedParticles;
    public MeshRenderer render;

    public Animator collectedAnimation;

    private void Start() {
        collectedParticles.Pause();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collectedAnimation.Play("MemoryCollected");
        hoveringParticles.Stop();
        collectedParticles.Play();
    }

    private void FixedUpdate() {
        render.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
    }
}
