using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{

    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID energeticParameter;
    private PARAMETER_ID relaxedParameter;

    [Header("Relaxed Value")]
    [Range(0f, 1f)]
    public float relaxedValue = 1f;

    [Header("Energetic Value")]
    [Range(0f, 1f)]
    public float energeticValue = 1f;

    private bool gameHasStarted = false;
    private float lerpValue = 0.1f;

    private void Start()
    {

        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription relaxedDescription;
        audio.getDescription(out relaxedDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION relaxedParameterDescription;
        relaxedDescription.getParameterDescriptionByName("Relaxed", out relaxedParameterDescription);
        relaxedParameter = relaxedParameterDescription.id;

        FMOD.Studio.EventDescription energeticDescription;
        audio.getDescription(out energeticDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION energeticParameterDescription;
        energeticDescription.getParameterDescriptionByName("Energetic", out energeticParameterDescription);
        energeticParameter = energeticParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }
    }
    private void FixedUpdate()
    {
        audio.setParameterByID(relaxedParameter, relaxedValue);
        audio.setParameterByID(energeticParameter, energeticValue);
        if (gameHasStarted)
        {
            energeticValue = Mathf.Lerp(energeticValue, 0, lerpValue * Time.fixedDeltaTime);
        }
        audio.setParameterByID(energeticParameter, energeticValue);
    }

    public void GameStarted()
    {
        gameHasStarted = true;
    }
}
