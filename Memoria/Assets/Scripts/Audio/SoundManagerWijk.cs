using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerWijk : MonoBehaviour {
    public BusEngineSound busEngineSound;
    private Tween engineStatusFadeTween;
    private Tween volumeFadeTween;
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

    public void FadeEngineVolume(float volumeValue, float duration = 2.5f) {
        volumeFadeTween?.Kill();
        if (duration == 0.0f) {
            busEngineSound.SetVolume(volumeValue);
        } else {
            volumeFadeTween = DOTween
                .To(() => busEngineSound.GetVolume(), x => busEngineSound.SetVolume(x), volumeValue, 1f)
                .SetEase(Ease.Linear);
        }
    }
}
