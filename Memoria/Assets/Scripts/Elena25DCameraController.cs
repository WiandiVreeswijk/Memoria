using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Elena25DCameraController : MonoBehaviour {
    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer transposer;
    public float minPlayerHeight = 0.0f;
    public float maxPlayerHeight = 8.0f;
    public float minCameraY = 0.9f;
    public float maxCameraY = 0.1f;
    private float cameraZoomIn = 10.0f;
    private float cameraZoomOut = 15.0f;

    void Start() {
        cam = Utils.FindUniqueObject<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update() {
        float mapped = Utils.Remap(Mathf.Clamp(transform.position.y, minPlayerHeight, maxPlayerHeight), minPlayerHeight, maxPlayerHeight, minCameraY, maxCameraY);
        transposer.m_ScreenY = Mathf.Lerp(transposer.m_ScreenY, mapped, Time.deltaTime);
        //transposer.m_ScreenY;
    }

    Tween zoomTween;
    private void SetZoom(float zoom) {
        zoomTween?.Kill();
        zoomTween = DOTween.To(() => transposer.m_CameraDistance, x => transposer.m_CameraDistance = x, zoom, 0.5f).SetEase(Ease.OutQuart);
    }

    public void ZoomIn() {
        SetZoom(cameraZoomIn);
    }

    public void ZoomOut() {
        SetZoom(cameraZoomOut);
    }
}
