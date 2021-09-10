using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAmbientSound : MonoBehaviour
{
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID insideParameter;

    [Header("Inside Status Options")]
    [Range(0f, 1f)]
    public float insideValue = 1f;

    private void Start()
    {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription insideDescription;
        audio.getDescription(out insideDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION insideParameterDescription;
        insideDescription.getParameterDescriptionByName("Inside", out insideParameterDescription);
        insideParameter = insideParameterDescription.id;


        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }

        SetInsideStatus(1f);
    }
    private void FixedUpdate()
    {
        audio.setParameterByID(insideParameter, insideValue);
    }
    public void SetParameters(float volume)
    {
        insideValue = volume;
        audio.setParameterByID(insideParameter, insideValue);
    }

    public void SetInsideStatus(float volume)
    {
        insideValue = volume;
        audio.setParameterByID(insideParameter, insideValue);
    }

    public float GetInsideStatus()
    {
        return insideValue;
    }
}
