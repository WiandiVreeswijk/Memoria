using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientControlChasingLevel : MonoBehaviour
{
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;

    [Header("Volume Value")]
    [Range(0f, 1f)]
    public float volumeValue = 0f;

    void Start()
    {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("AmbienceVolumeChasing", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }
    }

    public void SetVolume(float volume)
    {
        volumeValue = volume;
        audio.setParameterByID(volumeParameter, volume);
    }

    public float GetVolume()
    {
        return volumeValue;
    }
}
