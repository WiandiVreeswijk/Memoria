using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class FollowDollyTrack : MonoBehaviour {
    public CinemachineVirtualCamera cam;
    public float duration = 2f;
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DOTween.To(() => cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition,
                x => cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = x, 1.0f, duration);
        }
    }
}