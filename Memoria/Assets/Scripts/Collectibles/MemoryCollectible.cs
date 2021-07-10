using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCollectible : MonoBehaviour, IEnterActivatable {
    private bool triggered = false;
    public float rotationSpeed = 2f;

    public ParticleSystem hoveringParticles, collectedParticles;
    public ParticleSystem portal;
    public MeshRenderer render;

    public Animator collectedAnimation;

    private void Start() {
        collectedParticles.Pause();
    }

    private void FixedUpdate() {
        render.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
    }

    public void ActivateEnter() {
        if (!triggered) {
            collectedAnimation.Play("MemoryCollected");
            hoveringParticles.Stop();
            collectedParticles.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFXChasingLevel/MemoryCollected");
            Utils.DelayedAction(1.0f, () => portal.Play());
            triggered = true;
        }
    }
}
