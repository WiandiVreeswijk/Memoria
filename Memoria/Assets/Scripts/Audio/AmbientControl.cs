using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AmbientControl : MonoBehaviour {
    private PlayerMovementAdventure adventureMovement;

    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;
    private PARAMETER_ID birdsParameter;

    [Header("Volume Options")]
    [Range(0f, 1f)]
    public float volumeValue = 1f;

    [Header("Bird Options")]
    [Range(0f, 1f)]
    public float birdValue = 1f;

    private void Start() {
        adventureMovement = FindObjectOfType<PlayerMovementAdventure>();

        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("AmbientVolume", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;

        FMOD.Studio.EventDescription birdDescription;
        audio.getDescription(out birdDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION birdParameterDescription;
        birdDescription.getParameterDescriptionByName("Birds", out birdParameterDescription);
        birdsParameter = birdParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            audio.start();
        }

        SetVolume(0.7f);
        SetBirds(0.7f);
    }

    public void SetParameters(float volume, float birds) {
        volumeValue = volume;
        birdValue = birds;
        audio.setParameterByID(volumeParameter, volumeValue);
        audio.setParameterByID(birdsParameter, birdValue);
    }

    public void SetVolume(float volume) {
        volumeValue = volume;
        audio.setParameterByID(volumeParameter, volumeValue);
    }

    public void SetBirds(float birds) {
        birdValue = birds;
        audio.setParameterByID(birdsParameter, birdValue);
    }

    public float GetVolume() {
        float volume;
        audio.getParameterByID(volumeParameter, out volume);
        return volume;
    }

    public float GetBirds() {
        float volume;
        audio.getParameterByID(birdsParameter, out volume);
        return volume;
    }
}
