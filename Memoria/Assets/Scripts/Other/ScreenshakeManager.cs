using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

//#Todo this is very barebones. Probably won't work on scene switch and should definitely be improved
public class ScreenshakeManager : MonoBehaviour {
    [HideInInspector] public float rumble = 0.0f;

    private Tween tween;
    private GameObject vCamObject;
    private CinemachineBasicMultiChannelPerlin noise;

    public void OnGlobalsInitializeType(Globals.GlobalsType previousGlobalsType, Globals.GlobalsType currentGlobalsType) {
        if (noise != null) {
            noise.m_AmplitudeGain = 0;
        }

        switch (currentGlobalsType) {
            case Globals.GlobalsType.NEIGHBORHOOD: GetNoiseFromCamera(Globals.Player.CameraController.GetFirstPersonCamera()); break;
            case Globals.GlobalsType.OBLIVION: GetNoiseFromCamera(Globals.Player.CameraController.Get25DCamera()); break;
            case Globals.GlobalsType.MUDDLE: GetNoiseFromCamera(Globals.Player.CameraController.Get25DCamera()); break;
        }
    }

    public void Shake(float intensity, float duration) {
        if (noise == null) return;
        tween?.Kill(true);
        noise.m_AmplitudeGain = intensity;
        tween = DOTween.To(() => noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0.0f, duration)
            .SetEase(Ease.InExpo).OnComplete(() => tween = null);
    }

    public void Update() {
        if (tween == null && noise != null) {
            noise.m_AmplitudeGain = rumble;
        }
    }

    void GetNoiseFromCamera(CinemachineVirtualCamera camera) {
        CinemachineVirtualCamera cvc = camera.GetComponent<CinemachineVirtualCamera>();
        if (cvc != null) noise = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    //void UpdateCameraIfNecessary() {
    //    var brain = CinemachineCore.Instance.GetActiveBrain(0);
    //    if (vCamObject != brain.ActiveVirtualCamera.VirtualCameraGameObject) {
    //        tween?.Kill(true);
    //        vCamObject = brain.ActiveVirtualCamera.VirtualCameraGameObject;
    //        var camera = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    //        if (camera != null) noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    //        else {
    //            if (noise != null) noise.m_AmplitudeGain = rumble;
    //            noise = null;
    //        }
    //    }
    //}
}