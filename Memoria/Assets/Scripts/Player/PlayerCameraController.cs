using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;

public class PlayerCameraController : MonoBehaviour {
    //3D
    public CinemachineVirtualCamera firstPersonCamera;
    public GameObject arm;
    public bool isInFirstPerson;
    //2D
    public CinemachineVirtualCamera cam;
    public GameObject cameraTarget;

    public enum CameraType {
        FIRSTPERSON,
        THIRDPERSON
    }

    public void Start() {
        //arm.SetActive(false);
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
            Globals.Player.VisualEffects.SetMeshEnabled(false);
            tween = Utils.DelayedAction(2, () => arm.SetActive(true));
            isInFirstPerson = true;
        } else {
            firstPersonCamera.Priority = 9;
            Globals.Player.VisualEffects.SetMeshEnabled(true);
            arm.SetActive(false);
            isInFirstPerson = false;
        }
    }

    public bool IsInFirstPerson() {
        return isInFirstPerson;
    }
}
