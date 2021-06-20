using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControlChaseLevel : MonoBehaviour {

    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID intensityParameter;
    private PARAMETER_ID deathParameter;

    [Header("Intensity Value")]
    [Range(0f, 1f)]
    public float intensityValue = 0f;

    [Header("Death Value")]
    [Range(0f, 1f)]
    public float deathValue = 0f;

    private void Start() {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription intensityDescription;
        audio.getDescription(out intensityDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION intensityParameterDescription;
        intensityDescription.getParameterDescriptionByName("Intensity", out intensityParameterDescription);
        intensityParameter = intensityParameterDescription.id;

        FMOD.Studio.EventDescription deathDescription;
        audio.getDescription(out deathDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION deathParameterDescription;
        deathDescription.getParameterDescriptionByName("Death", out deathParameterDescription);
        deathParameter = deathParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            audio.start();
        }
    }

    //private void FixedUpdate() {
    //    audio.setParameterByID(intensityParameter, intensityValue);
    //    audio.setParameterByID(deathParameter, deathValue);
    //    if (gameHasStarted) {
    //        deathValue = Mathf.Lerp(deathValue, 0, lerpValue * Time.fixedDeltaTime);
    //    }
    //    audio.setParameterByID(deathParameter, deathValue);
    //}

    public void SetIntensity(float intensity) {
        intensityValue = intensity;
        audio.setParameterByID(intensityParameter, intensity);
    }

    public void SetDeath(float death) {
        deathValue = death;
        audio.setParameterByID(deathParameter, death);
    }

    public float GetIntensity() {
        return intensityValue;
    }

    public float GetDeath() {
        return deathValue;
    }
}
