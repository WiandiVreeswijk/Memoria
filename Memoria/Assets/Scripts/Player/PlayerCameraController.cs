using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;

public class PlayerCameraController : MonoBehaviour {
    //3D
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineFreeLook thirdPersonCamera;
    public GameObject arm;
    public bool isInFirstPerson;
    public bool inputEnabled = true;
    //2D
    public CinemachineVirtualCamera cam;
    public GameObject cameraTarget;

    public enum CameraType {
        FIRSTPERSON,
        THIRDPERSON
    }

    public void SetCameraTargetPosition(Vector3 position) {
        cameraTarget.transform.position = position;
    }

    public void ToggleCamera() {
        ChangeCamera(isInFirstPerson ? CameraType.THIRDPERSON : CameraType.FIRSTPERSON);
        //arm.transform.DORotate(new Vector3(90, 0, 0), 0);
    }

    Tween tween;
    public void ChangeCamera(CameraType type) {
        tween?.Kill();
        if (type == CameraType.FIRSTPERSON) {
            firstPersonCamera.Priority = 11;
            thirdPersonCamera.Priority = 9;
            tween = Utils.DelayedAction(2, () => {
                if (arm != null) arm.SetActive(true);
            });
            isInFirstPerson = true;
        } else {
            thirdPersonCamera.Priority = 11;
            firstPersonCamera.Priority = 9;
            if (arm != null) arm.SetActive(false);
            isInFirstPerson = false;
        }
    }

    public bool IsInFirstPerson() {
        return isInFirstPerson;
    }
}
