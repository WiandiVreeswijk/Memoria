using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerWijk : MonoBehaviour {
    public BusEngineSound busEngineSound;
    private RainAmbientSound rainAmbientSound;
    private AmbientControl ambientControl;
    private Tween engineStatusFadeTween;
    private Tween engineVolumeFadeTween, rainVolumeFadeTween, ambientVolumeFadeTween;

    private void Start()
    {
        rainAmbientSound = GetComponent<RainAmbientSound>();
        ambientControl = GetComponent<AmbientControl>();
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
        engineVolumeFadeTween?.Kill();
        if (duration == 0.0f) {
            busEngineSound.SetVolume(volumeValue);
        } else {
            engineVolumeFadeTween = DOTween
                .To(() => busEngineSound.GetVolume(), x => busEngineSound.SetVolume(x), volumeValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeRainStatus(float insideValue, float duration = 5f)
    {
        rainVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            rainAmbientSound.SetInsideStatus(insideValue);
        }
        else
        {
            rainVolumeFadeTween = DOTween
                .To(() => rainAmbientSound.GetInsideStatus(), x => rainAmbientSound.SetInsideStatus(x), insideValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeAmbientVolume(float volume, float duration = 5f)
    {
        ambientVolumeFadeTween?.Kill();
        if (duration == 0.0f)
        {
            ambientControl.SetVolume(volume);
        }
        else
        {
            ambientVolumeFadeTween = DOTween
                .To(() => ambientControl.GetVolume(), x => ambientControl.SetVolume(x), volume, 1f)
                .SetEase(Ease.Linear);
        }
    }


    //signal emitters for timeline
    #region signal emitters
    public void FadeToIdle()
    {
        FadeEngineStatus(0f, 2f);
        FadeRainStatus(0, 2f);
        FadeAmbientVolume(1.0f, 5.0f);
    }

    public void FadeToDriving()
    {
        FadeEngineStatus(1f, 2f);
    }
    public void FadeToNothing()
    {
        FadeEngineVolume(0f, 10f);
        Globals.AmbientControl.SetBirds(0.9f);
        Globals.AmbientControl.FadeAmbientVolume(0.95f, 4.0f);
    }
    #endregion
}
