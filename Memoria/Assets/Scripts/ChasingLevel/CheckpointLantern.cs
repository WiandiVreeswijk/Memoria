using System.Collections.Generic;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine;

public class CheckpointLantern : MonoBehaviour, IEnterActivatable {
    public Light lightInLantern;
    public Light lightInSafeArea;
    public ParticleSystem particles;
    public ParticleSystem embers;

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

    //FMOD
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;

    [Header("Burning Volume")]
    [Range(0f, 1f)]
    public float volumeValue = 0f;
    public float volumeMultiplier;

    public void Start() {
        lightInLantern.gameObject.SetActive(false);
        lightInSafeArea.gameObject.SetActive(false);
        smoothQueue = new Queue<float>(smoothing);

        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("BurningVolume", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;
    }

    public void ActivateEnter() {
        if (!activated) {
            activated = true;
            particles.Play();
            embers.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFXChasingLevel/CheckpointBurstFlame");
            FMOD.Studio.PLAYBACK_STATE PbState;
            audio.getPlaybackState(out PbState);
            if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                audio.start();
            }
            lightInLantern.gameObject.SetActive(true);
            lightInSafeArea.gameObject.SetActive(true);
            lightInLantern.intensity = 0;
            lightInSafeArea.intensity = 0;

            DOTween.To(() => lightInLantern.intensity, x => {
                lightInLantern.intensity = x;
                lightInSafeArea.intensity = x;
            }, 3.0f, 0.2f).SetEase(Ease.InBounce)
                .OnComplete(() => {
                    igniting = false;
                });
        }
    }
    private void FixedUpdate()
    {
        float distance = Utils.Distance(Globals.Player.transform.position.x, transform.position.x);
        volumeValue = Mathf.Lerp(1.0f, 0, distance * volumeMultiplier);
        audio.setParameterByID(volumeParameter, volumeValue);
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
        lightInLantern.intensity = lastSum / (float)smoothQueue.Count;
        lightInSafeArea.intensity = lightInLantern.intensity;
    }

    public void EmitRespawnParticles() {
        embers.Play();
    }
}
