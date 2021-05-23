using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AmbientControl : MonoBehaviour
{
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

    private void Start()
    {
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
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }
    }
    private void FixedUpdate()
    {
        if (adventureMovement.inHouse)
        {
            audio.setParameterByID(volumeParameter, 0);
        }
        else if(!adventureMovement.inHouse)
        {
            audio.setParameterByID(volumeParameter, volumeValue);
            audio.setParameterByID(birdsParameter, birdValue);
        }
    }
}
