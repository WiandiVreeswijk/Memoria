using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerWijk : MonoBehaviour {
    public BusEngineSound busEngineSound;
    private RainAmbientSound rainAmbientSound;
    private Tween engineStatusFadeTween;
    private Tween volumeFadeTween;

    private void Start()
    {
        rainAmbientSound = GetComponent<RainAmbientSound>();
    }
    public void FadeEngineStatus(float engineValue, float duration = 2.5f) {
        engineStatusFadeTween?.Kill();
        if (duration == 0.0f) {
            busEngineSound.SetEngineStatus(engineValue);
        } else {
            engineStatusFadeTween = DOTween
                .To(() => busEngineSound.GetEngineStatus(), x => busEngineSound.SetEngineStatus(x), engineValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeEngineVolume(float volumeValue, float duration = 5f) {
        volumeFadeTween?.Kill();
        if (duration == 0.0f) {
            busEngineSound.SetVolume(volumeValue);
        } else {
            volumeFadeTween = DOTween
                .To(() => busEngineSound.GetVolume(), x => busEngineSound.SetVolume(x), volumeValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeRainStatus(float insideValue, float duration = 5f)
    {
        volumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            rainAmbientSound.SetInsideStatus(insideValue);
        }
        else
        {
            volumeFadeTween = DOTween
                .To(() => rainAmbientSound.GetInsideStatus(), x => rainAmbientSound.SetInsideStatus(x), insideValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeToIdle()
    {
        FadeEngineStatus(0f, 2f);
        FadeRainStatus(0, 2f);
    }

    public void FadeToDriving()
    {
        FadeEngineStatus(1f, 2f);
    }
    public void FadeToNothing()
    {
        FadeEngineVolume(0f, 10f);
    }

}
