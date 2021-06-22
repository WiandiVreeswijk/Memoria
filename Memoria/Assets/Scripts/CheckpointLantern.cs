using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CheckpointLantern : MonoBehaviour, IActivatable {
    public GameObject lightObject;
    public ParticleSystem particles;
    public ParticleSystem embers;

    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public new Light light;
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;

    private bool activated = false;
    private bool igniting = true;
    public void Start() {
        lightObject.SetActive(false);

        smoothQueue = new Queue<float>(smoothing);
        // External or internal light?
        if (light == null) {
            light = lightObject.GetComponent<Light>();
        }
    }

    public void Activate() {
        if (!activated) {
            lightObject.SetActive(true);
            particles.Play();

            activated = true;
            Light light = lightObject.GetComponent<Light>();
            light.intensity = 0;
            embers.Play();
            DOTween.To(() => light.intensity, x => light.intensity = x, 3.0f, 0.2f).SetEase(Ease.InBounce)
                .OnComplete(() => {
                    igniting = false;
                });
        }
    }

    public void Update() {
        //if (Input.GetKey(KeyCode.R)) {
        //    igniting = true;
        //    activated = false;
        //    Activate();
        //}


        if (igniting) return;
        while (smoothQueue.Count >= smoothing) {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        light.intensity = lastSum / (float)smoothQueue.Count;
    }

    public void EmitRespawnParticles() {
        embers.Play();
    }
}
