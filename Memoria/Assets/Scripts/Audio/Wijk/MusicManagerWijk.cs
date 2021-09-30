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
    private PARAMETER_ID CelloVolumeParameter, ViolinVolumeParameter, PianoVolumeParameter, BassVolumeParameter, HarpVolumeParameter, FluteVolumeParameter;

    #region VolumeValue definitions
    [Header("CelloVolume Value")]
    [Range(0f, 1f)]
    public float celloVolumeValue = 1f;

    [Header("ViolinVolume Value")]
    [Range(0f, 1f)]
    public float violinVolumeValue = 1f;

    [Header("PianoVolume Value")]
    [Range(0f, 1f)]
    public float pianoVolumeValue = 1f;

    [Header("BassVolume Value")]
    [Range(0f, 1f)]
    public float bassVolumeValue = 1f;

    [Header("HarpVolume Value")]
    [Range(0f, 1f)]
    public float harpVolumeValue = 1f;

    [Header("FluteVolume Value")]
    [Range(0f, 1f)]
    public float fluteVolumeValue = 1f;

    #endregion

    private bool gameHasStarted = false;
    private float lerpValue = 0.1f;

    private Tween celloVolumeFadeTween, violinVolumeFadeTween, pianoVolumeFadeTween, bassVolumeFadeTween, harpVolumeFadeTween, fluteVolumeFadeTween;

    private void Start()
    {

        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        #region defining parameters
        FMOD.Studio.EventDescription celloVolumeDescription;
        audio.getDescription(out celloVolumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION celloVolumeParameterDescription;
        celloVolumeDescription.getParameterDescriptionByName("CelloVolume", out celloVolumeParameterDescription);
        CelloVolumeParameter = celloVolumeParameterDescription.id;

        FMOD.Studio.EventDescription violinVolumeDescription;
        audio.getDescription(out violinVolumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION violinVolumeParameterDescription;
        violinVolumeDescription.getParameterDescriptionByName("ViolinVolume", out violinVolumeParameterDescription);
        ViolinVolumeParameter = violinVolumeParameterDescription.id;

        FMOD.Studio.EventDescription pianoVolumeDescription;
        audio.getDescription(out pianoVolumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION pianoVolumeParameterDescription;
        pianoVolumeDescription.getParameterDescriptionByName("PianoVolume", out pianoVolumeParameterDescription);
        PianoVolumeParameter = pianoVolumeParameterDescription.id;

        FMOD.Studio.EventDescription bassVolumeDescription;
        audio.getDescription(out bassVolumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION bassVolumeParameterDescription;
        bassVolumeDescription.getParameterDescriptionByName("BassVolume", out bassVolumeParameterDescription);
        BassVolumeParameter = bassVolumeParameterDescription.id;

        FMOD.Studio.EventDescription harpVolumeDescription;
        audio.getDescription(out harpVolumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION harpVolumeParameterDescription;
        harpVolumeDescription.getParameterDescriptionByName("HarpVolume", out harpVolumeParameterDescription);
        HarpVolumeParameter = harpVolumeParameterDescription.id;

        FMOD.Studio.EventDescription fluteVolumeDescription;
        audio.getDescription(out fluteVolumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION fluteVolumeParameterDescription;
        fluteVolumeDescription.getParameterDescriptionByName("FluteVolume", out fluteVolumeParameterDescription);
        FluteVolumeParameter = fluteVolumeParameterDescription.id;

        #endregion

        FMOD.Studio.PLAYBACK_STATE PbState;
        audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }
    }
    private void FixedUpdate()
    {
        #region updating parameters
        audio.setParameterByID(CelloVolumeParameter, celloVolumeValue);
        audio.setParameterByID(ViolinVolumeParameter, violinVolumeValue);
        audio.setParameterByID(PianoVolumeParameter, pianoVolumeValue);
        audio.setParameterByID(BassVolumeParameter, bassVolumeValue);
        audio.setParameterByID(HarpVolumeParameter, harpVolumeValue);
        audio.setParameterByID(FluteVolumeParameter, fluteVolumeValue);
        #endregion
    }
    #region fadeVolume functions
    public void FadeCelloVolume(float celloVolumeValue, float duration = 4f)
    {
        celloVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetCelloVolume(celloVolumeValue);
        }
        else
        {
            celloVolumeFadeTween = DOTween
                .To(() => GetCelloVolume(), x => SetCelloVolume(x), celloVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    public void FadeViolinVolume(float violinVolumeValue, float duration = 4f)
    {
        violinVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetViolinVolume(violinVolumeValue);
        }
        else
        {
            violinVolumeFadeTween = DOTween
                .To(() => GetViolinVolume(), x => SetViolinVolume(x), violinVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    public void FadePianoVolume(float pianoVolumeValue, float duration = 4f)
    {
        pianoVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetPianoVolume(pianoVolumeValue);
        }
        else
        {
            pianoVolumeFadeTween = DOTween
                .To(() => GetPianoVolume(), x => SetPianoVolume(x), pianoVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    public void FadeBassVolume(float bassVolumeValue, float duration = 4f)
    {
        bassVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetBassVolume(bassVolumeValue);
        }
        else
        {
            bassVolumeFadeTween = DOTween
                .To(() => GetBassVolume(), x => SetBassVolume(x), bassVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    public void FadeHarpVolume(float harpVolumeValue, float duration = 4f)
    {
        harpVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetHarpVolume(harpVolumeValue);
        }
        else
        {
            harpVolumeFadeTween = DOTween
                .To(() => GetHarpVolume(), x => SetHarpVolume(x), harpVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    public void FadeFluteVolume(float fluteVolumeValue, float duration = 4f)
    {
        fluteVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            SetFluteVolume(fluteVolumeValue);
        }
        else
        {
            fluteVolumeFadeTween = DOTween
                .To(() => GetFluteVolume(), x => SetFluteVolume(x), fluteVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }

    #endregion



    #region Setters
    public void SetCelloVolume(float volume)
    {
        celloVolumeValue = volume;
        audio.setParameterByID(CelloVolumeParameter, celloVolumeValue);
    }

    public void SetViolinVolume(float volume)
    {
        violinVolumeValue = volume;
        audio.setParameterByID(ViolinVolumeParameter, violinVolumeValue);
    }

    public void SetPianoVolume(float volume)
    {
        pianoVolumeValue = volume;
        audio.setParameterByID(PianoVolumeParameter, pianoVolumeValue);
    }

    public void SetBassVolume(float volume)
    {
        bassVolumeValue = volume;
        audio.setParameterByID(BassVolumeParameter, bassVolumeValue);
    }
    public void SetHarpVolume(float volume)
    {
        harpVolumeValue = volume;
        audio.setParameterByID(HarpVolumeParameter, harpVolumeValue);
    }
    public void SetFluteVolume(float volume)
    {
        fluteVolumeValue = volume;
        audio.setParameterByID(FluteVolumeParameter, fluteVolumeValue);
    }
    #endregion

    #region Getters
    public float GetCelloVolume()
    {
        return celloVolumeValue;
    }

    public float GetViolinVolume()
    {
        return violinVolumeValue;
    }
    public float GetPianoVolume()
    {
        return pianoVolumeValue;
    }
    public float GetBassVolume()
    {
        return bassVolumeValue;
    }
    public float GetHarpVolume()
    {
        return harpVolumeValue;
    }
    public float GetFluteVolume()
    {
        return fluteVolumeValue;
    }

    #endregion

    public void OpeningCutsceneEnd()
    {
        FadeCelloVolume(1.0f, 5.0f);
        FadeBassVolume(1.0f, 5.0f);
        FadeFluteVolume(0.9f, 5.0f);
        FadeViolinVolume(1.0f, 5.0f);
        FadePianoVolume(0.6f, 5.0f);
    }
    public void OpeningCutSceneFadeToNothing()
    {
        FadeFluteVolume(0.0f, 5.0f);
        FadeViolinVolume(0.0f, 5.0f);
        FadePianoVolume(0.6f, 5.0f);
    }

    public void FadeToMuteAll()
    {
        FadeBassVolume(0, 1);
        FadeCelloVolume(0, 1);
        FadeFluteVolume(0, 1);
        FadeHarpVolume(0, 1);
        FadePianoVolume(0, 1);
        FadeViolinVolume(0, 1);
    }
}
