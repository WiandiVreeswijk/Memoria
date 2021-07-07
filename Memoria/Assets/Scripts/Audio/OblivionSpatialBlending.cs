using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OblivionSpatialBlending : MonoBehaviour {
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;

    [Header("Oblivion Volume")]
    [Range(0f, 1f)]
    public float volumeValue = 0f;
    public float volumeMultiplier;

    private void Start() {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("OblivionVolume", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            audio.start();
        }
    }

    private void FixedUpdate() {
        bool isBehindOblivion = Globals.Player.transform.position.x < Globals.OblivionManager.GetOblivionPosition();
        float distance = isBehindOblivion ? 0.0f : Utils.Distance(Globals.Player.transform.position.x, Globals.OblivionManager.GetOblivionPosition());
        volumeValue = Mathf.Lerp(1.0f, 0, distance * volumeMultiplier);
        audio.setParameterByID(volumeParameter, volumeValue);
    }
}
