using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

//#Todo this is very barebones. Probably won't work on scene switch and should definitely be improved
public class Screenshake : MonoBehaviour {
    Tween tween;
    public float rumble = 0.0f;
    private CinemachineBasicMultiChannelPerlin p;

    public void Start() {
        //How to handle cinemachine camera switches?
        var brain = CinemachineCore.Instance.GetActiveBrain(0);
        var Vcam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        p = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Shake(float intensity, float duration) {
        tween?.Kill(true);
        p.m_AmplitudeGain = intensity;
        tween = DOTween.To(() => p.m_AmplitudeGain, x => p.m_AmplitudeGain = x, 0.0f, duration).SetEase(Ease.InExpo).OnComplete(() => tween = null);
    }

    public void Update() {
        if (tween == null) {
            p.m_AmplitudeGain = rumble;
        }
    }
}
