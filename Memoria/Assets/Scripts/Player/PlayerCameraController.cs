using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerCameraController : MonoBehaviour {
    public CinemachineVirtualCamera cam;
    public GameObject cameraTarget;

    public void SetCameraTargetPosition(Vector3 position) {
        cameraTarget.transform.position = position;
    }
}
