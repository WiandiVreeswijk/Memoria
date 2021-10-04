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
    private PARAMETER_ID chorusParameter;
    private PARAMETER_ID volumeParameter;

    [Header("Volume Value")]
    [Range(0f, 1f)]
    public float volumeValue = 0f;

    [Header("Intensity Value")]
    [Range(0f, 1f)]
    public float intensityValue = 0f;

    [Header("Death Value")]
    [Range(0f, 1f)]
    public float deathValue = 0f;

    [Header("Chorus Value")]
    [Range(0f, 1f)]
    public float chorusValue = 0f;

    public GameObject memory;

    private void Start() {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("VolumeChasingLevelMusic", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;

        FMOD.Studio.EventDescription intensityDescription;
        audio.getDescription(out intensityDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION intensityParameterDescription;
        intensityDescription.getParameterDescriptionByName("BeingChased", out intensityParameterDescription);
        intensityParameter = intensityParameterDescription.id;

        FMOD.Studio.EventDescription deathDescription;
        audio.getDescription(out deathDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION deathParameterDescription;
        deathDescription.getParameterDescriptionByName("Death", out deathParameterDescription);
        deathParameter = deathParameterDescription.id;

        FMOD.Studio.EventDescription chorusDescription;
        audio.getDescription(out chorusDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION chorusParameterDescription;
        chorusDescription.getParameterDescriptionByName("NearingEnd", out chorusParameterDescription);
        chorusParameter = chorusParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            audio.start();
        }
    }

    private void FixedUpdate()
    {
        float distance = Utils.Distance(Globals.Player.transform.position.x, memory.transform.position.x);
        chorusValue = Mathf.Lerp(1.0f, 0, distance * 0.005f);
        audio.setParameterByID(chorusParameter, chorusValue);
    }

    public void SetVolume(float volume)
    {
        volumeValue = volume;
        audio.setParameterByID(volumeParameter, volume);
    }
    public void SetIntensity(float intensity) {
        intensityValue = intensity;
        audio.setParameterByID(intensityParameter, intensity);
    }

    public void SetDeath(float death) {
        deathValue = death;
        audio.setParameterByID(deathParameter, death);
    }

    public void SetChorus(float chorus)
    {
        chorusValue = chorus;
        audio.setParameterByID(deathParameter, chorus);
    }

    public float GetVolume()
    {
        return volumeValue;
    }
    public float GetIntensity() {
        return intensityValue;
    }

    public float GetDeath() {
        return deathValue;
    }
    public float GetChorus()
    {
        return chorusValue;
    }
}
