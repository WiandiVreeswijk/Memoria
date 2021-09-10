using DG.Tweening;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerWijk : MonoBehaviour
{
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID lowEnergyParameter;

    [Header("LowEnergy Value")]
    [Range(0f, 1f)]
    public float lowEnergyValue = 1f;

    private bool gameHasStarted = false;
    private float lerpValue = 0.1f;

    private Tween volumeFadeTween;

    private void Start()
    {

        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription lowEnergyDescription;
        audio.getDescription(out lowEnergyDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION lowEnergyParameterDescription;
        lowEnergyDescription.getParameterDescriptionByName("LowEnergy(Wijk)", out lowEnergyParameterDescription);
        lowEnergyParameter = lowEnergyParameterDescription.id;

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }
    }
    private void FixedUpdate()
    {
        audio.setParameterByID(lowEnergyParameter, lowEnergyValue);
    }

    public void FadeMusicVolume(float engineValue, float duration = 4f)
    {
        volumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetVolume(engineValue);
        }
        else
        {
            volumeFadeTween = DOTween
                .To(() => GetVolume(), x => SetVolume(x), engineValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void SetVolume(float volume)
    {
        lowEnergyValue = volume;
        audio.setParameterByID(lowEnergyParameter, lowEnergyValue);
    }

    public float GetVolume()
    {
        return lowEnergyValue;
    }
}
