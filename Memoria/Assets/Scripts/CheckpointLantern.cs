using System.Collections.Generic;
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
            embers.Play();
            activated = true;
        }
    }

    public void Update() {
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
