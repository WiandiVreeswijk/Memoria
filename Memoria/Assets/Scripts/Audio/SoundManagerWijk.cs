using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerWijk : MonoBehaviour
{
    BusEngineSound busEngineSound;

    private Tween intensityFadeTween;
    public void FadeEngineStatus(float engineValue, float duration = 2.5f)
    {
        intensityFadeTween?.Kill();
        if (duration == 0.0f)
        {
            busEngineSound.SetEngineStatus(engineValue);
        }
        else
        {
            intensityFadeTween = DOTween
                .To(() => busEngineSound.GetEngineStatus(), x => busEngineSound.SetEngineStatus(x), engineValue, 1f)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeVolume(float volumeValue, float duration = 2.5f)
    {
        intensityFadeTween?.Kill();
        if (duration == 0.0f)
        {
            busEngineSound.SetVolume(volumeValue);
        }
        else
        {
            intensityFadeTween = DOTween
                .To(() => busEngineSound.GetEngineStatus(), x => busEngineSound.SetVolume(x), volumeValue, 1f)
                .SetEase(Ease.Linear);
        }
    }
}
