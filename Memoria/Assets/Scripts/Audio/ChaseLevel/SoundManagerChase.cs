using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManagerChase : MonoBehaviour {
    private MusicControlChaseLevel musicControl;
    private Tween volumeFadeTween;
    private Tween intensityFadeTween;
    private Tween deathFadeTween;

    private void Start() {
        musicControl = GetComponent<MusicControlChaseLevel>();
    }
    public void FadeVolume(float volume, float duration = 2.5f)
    {
        intensityFadeTween?.Kill();
        if (duration == 0.0f)
        {
            musicControl.SetVolume(volume);
        }
        else
        {
            volumeFadeTween = DOTween
                .To(() => musicControl.GetVolume(), x => musicControl.SetVolume(x), volume, duration)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeIntensity(float intensity, float duration = 2.5f) {
        intensityFadeTween?.Kill();
        if (duration == 0.0f) {
            musicControl.SetIntensity(intensity);
        } else {
            intensityFadeTween = DOTween
                .To(() => musicControl.GetIntensity(), x => musicControl.SetIntensity(x), intensity, duration)
                .SetEase(Ease.Linear);
        }
    }

    public void FadeDeath(float death, float duration = 0.5f) {
        deathFadeTween?.Kill();
        if (duration == 0.0f) {
            musicControl.SetDeath(death);
        } else {
            deathFadeTween = DOTween.To(() => musicControl.GetDeath(), x => musicControl.SetDeath(x), death, duration)
                .SetEase(Ease.Linear);
        }
    }
}