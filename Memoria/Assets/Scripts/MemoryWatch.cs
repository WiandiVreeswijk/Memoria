using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class MemoryWatch : MonoBehaviour {
    public GameObject bigArm;
    public GameObject smallArm;
    public WatchParticleSystem watchParticleSystem;
    public WatchReleaseParticleSystem releaseParticleSystem;

    public float shakeTimeScale = 1.0f;

    public float baseShakeDuration = 1.0f;
    public float baseShakeStrength = 0.1f;
    public int baseShakeVibrato = 10;

    public MeshRenderer watchRenderer;
    private Material watchMaterial;
    private Material watchEdgeMaterial;

    private float activity = 0.0f;
    private Color color1 = Color.gray;
    private Color color2 = Color.white;

    Tweener shakeTween;
    private MemoryObject memoryObject;

    public void Start() {
        watchMaterial = watchRenderer.materials.First(x => x.name.Contains("WatchMaterial"));
        watchEdgeMaterial = watchRenderer.materials.First(x => x.name.Contains("WatchEdgeMaterial"));
    }

    public void SetActivity(float activity, MemoryObject memoryObject) {
        this.activity = activity;
        this.memoryObject = memoryObject;
        shakeTween.Kill(true);
        shakeTween = DOTween.Shake(() => transform.localPosition, x => transform.localPosition = x, baseShakeDuration, baseShakeStrength * activity, baseShakeVibrato);
        watchMaterial.color = Color.Lerp(color1, color2 * 2, Globals.MemoryWatchManager.colorCurve.Evaluate(activity));
    }

    public void SetWatchEdgeProgress(float progress, bool isActive, bool shouldEmitParticles) {
        watchEdgeMaterial.SetFloat("_Rotation", progress);
        if (shouldEmitParticles && isActive) watchParticleSystem.Emit((1.0f - progress) / 10);
    }
    public void FixedUpdate() {
        bigArm.transform.Rotate(Vector3.up, 10.0f * Globals.MemoryWatchManager.rotationCurve.Evaluate(activity));
        smallArm.transform.Rotate(Vector3.up, 6.75f * activity);
    }

    public void Activate(MemoryObject memoryObject)
    {
        releaseParticleSystem.targetObject = memoryObject.gameObject;
    }
}
