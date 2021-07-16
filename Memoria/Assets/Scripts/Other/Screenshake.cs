using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

//#Todo this is very barebones. Probably won't work on scene switch and should definitely be improved
public class Screenshake : MonoBehaviour {
    public float rumble = 0.0f;

    private Tween tween;
    private GameObject vCamObject;
    private CinemachineBasicMultiChannelPerlin noise;

    public void Start() {
        UpdateCameraIfNecessary();
    }

    public void Shake(float intensity, float duration) {
        if (noise == null) return;
        tween?.Kill(true);
        noise.m_AmplitudeGain = intensity;
        tween = DOTween.To(() => noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0.0f, duration).SetEase(Ease.InExpo).OnComplete(() => tween = null);
    }

    public void Update() {
        UpdateCameraIfNecessary();
        if (tween == null && noise != null) {
            noise.m_AmplitudeGain = rumble;
        }
    }

    void UpdateCameraIfNecessary() {
        var brain = CinemachineCore.Instance.GetActiveBrain(0);
        if (vCamObject != brain.ActiveVirtualCamera.VirtualCameraGameObject) {
            tween?.Kill(true);
            vCamObject = brain.ActiveVirtualCamera.VirtualCameraGameObject;
            var camera = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            if (camera != null) noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            else {
                if(noise != null) noise.m_AmplitudeGain = rumble;
                noise = null;
            }
        }
    }
}
