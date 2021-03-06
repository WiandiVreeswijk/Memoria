using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class MemoryCollectible : MonoBehaviour, IEnterActivatable {
    private bool triggered = false;
    public float rotationSpeed = 2f;

    public ParticleSystem hoveringParticles, collectedParticles;
    public PortalVisual portal;
    public MeshRenderer render;
    public TrophyType trophyType;
    public Animator collectedAnimation;

    private void Start() {
        collectedParticles.Pause();
    }

    private void FixedUpdate() {
        render.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
    }

    public void ActivateEnter() {
        if (!triggered) {
            //#Todo OnLevelEnded should not be called here
            Globals.AnalyticsManager.OnLevelEnded();
            if(Globals.TrophyManager) Globals.TrophyManager.CollectTrophy(trophyType);
            collectedAnimation.Play("MemoryCollected");
            hoveringParticles.Stop();
            collectedParticles.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFXChasingLevel/MemoryCollected");
            Utils.DelayedAction(1.0f, () => portal.Open());
            triggered = true;
        }
    }
}
