using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;

public class PlayerCameraController : MonoBehaviour {
    //3D
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    private bool isInFirstPerson;
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
    }

    public void ChangeCamera(CameraType type) {
        if (type == CameraType.FIRSTPERSON) {
            firstPersonCamera.Priority = 11;
            thirdPersonCamera.Priority = 9;
            isInFirstPerson = true;
        } else {
            thirdPersonCamera.Priority = 11;
            firstPersonCamera.Priority = 9;
            isInFirstPerson = false;
        }
    }

    public bool IsInFirstPerson() {
        return isInFirstPerson;
    }

    public void ChangeCameraAndSetRotation(CameraType type, float x, float y) {
        if (type == CameraType.FIRSTPERSON) {
            firstPersonCamera.Priority = 11;
            thirdPersonCamera.Priority = 9;
            isInFirstPerson = true;
        } else {
            thirdPersonCamera.m_XAxis.Value = x;
            thirdPersonCamera.m_YAxis.Value = y;
            thirdPersonCamera.Priority = 11;
            firstPersonCamera.Priority = 9;
            isInFirstPerson = false;
        }
    }
}
