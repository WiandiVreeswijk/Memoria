using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCollectible : MonoBehaviour {
    public GameObject renderer;
    public float rotationSpeed = 2f;

    private Tween rotationTween, positionTween;

    public ParticleSystem hoveringParticles, collectedParticles;
    public MeshRenderer render;
    private void Start() {
        collectedParticles.Pause();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        render.enabled = false;
        hoveringParticles.Stop();
        collectedParticles.Play();
    }

    private void FixedUpdate() {
        renderer.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
    }
}
