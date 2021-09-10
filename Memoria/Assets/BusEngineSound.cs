using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusEngineSound : MonoBehaviour {
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;
    private PARAMETER_ID engineStatusParameter;

    [Header("Volume Options")]
    [Range(0f, 1f)]
    public float volumeValue = 1f;

    [Header("Engine Options")]
    [Range(0f, 1f)]
    public float engineValue = 1f;

    private void Start() {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("EngineVolume", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;

        FMOD.Studio.EventDescription engineStatusDescription;
        audio.getDescription(out engineStatusDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION engineStatusParameterDescription;
        engineStatusDescription.getParameterDescriptionByName("BusEngineStatus", out engineStatusParameterDescription);
        engineStatusParameter = engineStatusParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            audio.start();
        }

        SetVolume(0f);
        SetEngineStatus(0f);
    }
    private void FixedUpdate() {
        audio.setParameterByID(volumeParameter, volumeValue);
        audio.setParameterByID(engineStatusParameter, engineValue);
    }
    public void SetParameters(float volume, float engineStatus) {
        volumeValue = volume;
        engineValue = engineStatus;
        audio.setParameterByID(volumeParameter, volumeValue);
        audio.setParameterByID(engineStatusParameter, engineValue);
    }

    public void SetVolume(float volume) {
        volumeValue = volume;
        audio.setParameterByID(volumeParameter, volumeValue);
    }

    public void SetEngineStatus(float engineStatus) {
        engineValue = engineStatus;
        audio.setParameterByID(engineStatusParameter, engineValue);
    }

    public float GetVolume() {
        return volumeValue;
    }

    public float GetEngineStatus() {
        return engineValue;
    }
}
