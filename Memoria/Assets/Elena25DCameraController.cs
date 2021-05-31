using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Elena25DCameraController : MonoBehaviour {
    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer transposer;
    public float minPlayerHeight = 0.0f;
    public float maxPlayerHeight = 8.0f;
    public float minCameraY = 0.9f;
    public float maxCameraY = 0.1f;
    void Start() {
        cam = Utils.FindUniqueObject<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update() {
        float mapped = Utils.Map(Mathf.Clamp(transform.position.y, minPlayerHeight, maxPlayerHeight), minPlayerHeight, maxPlayerHeight, minCameraY, maxCameraY);
        transposer.m_ScreenY = Mathf.Lerp(transposer.m_ScreenY, mapped, Time.deltaTime);
        //transposer.m_ScreenY;
    }
}
