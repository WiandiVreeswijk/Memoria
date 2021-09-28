using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorIdleSound : MonoBehaviour
{
    //FMOD
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public string parameterName;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;

    [Header("Idle Volume")]
    [Range(0f, 1f)]
    public float volumeValue = 0f;
    public float volumeMultiplier;

    public bool mute;

    private void Start()
    {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName(parameterName, out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }
    }

    private void FixedUpdate()
    {
        SoundBehaviour();
    }

    private void SoundBehaviour()
    {
        if (!mute)
        {
            float distance = Vector3.Distance(Globals.Player.transform.position, transform.position);
            volumeValue = Mathf.Lerp(1.0f, 0, distance * volumeMultiplier);
            audio.setParameterByID(volumeParameter, volumeValue);
        }
        else
        {
            audio.setParameterByID(volumeParameter, 0);
        }
    }
}
