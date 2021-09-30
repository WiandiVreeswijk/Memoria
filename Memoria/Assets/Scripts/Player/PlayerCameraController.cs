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

    public bool IsInFirstPerson() {
        return isInFirstPerson;
    }

    Tween inputTween;
    public void ChangeCamera(CameraType type)
    {

        if (type == CameraType.FIRSTPERSON) {
            firstPersonCamera.Priority = 11;
            thirdPersonCamera.Priority = 9;
            isInFirstPerson = true;
        } else {
            inputTween?.Kill();
            Globals.CinemachineManager.SetInputEnabled(false);
            inputTween = Utils.DelayedAction(1.0f, () => Globals.CinemachineManager.SetInputEnabled(true));
            thirdPersonCamera.Priority = 11;
            firstPersonCamera.Priority = 9;
            isInFirstPerson = false;
        }
    }


    public void ChangeCameraAndSetRotation(CameraType type, float x, float y) {
        if (type == CameraType.FIRSTPERSON) {
            firstPersonCamera.Priority = 11;
            thirdPersonCamera.Priority = 9;
            isInFirstPerson = true;
        } else {
            inputTween?.Kill();
            Globals.CinemachineManager.SetInputEnabled(false);
            inputTween = Utils.DelayedAction(1.0f, () => Globals.CinemachineManager.SetInputEnabled(true));
            thirdPersonCamera.m_XAxis.Value = x;
            thirdPersonCamera.m_YAxis.Value = y;
            thirdPersonCamera.Priority = 11;
            firstPersonCamera.Priority = 9;
            isInFirstPerson = false;
        }
    }


    public void EaseFOV(float duration, float amount, bool snapBack, Ease easeIn, Ease easeOut = Ease.Linear) {
        if (!IsInFirstPerson()) return;
        float currentFOV = firstPersonCamera.m_Lens.FieldOfView;
        DOTween.To(() => firstPersonCamera.m_Lens.FieldOfView, x => firstPersonCamera.m_Lens.FieldOfView = x,
           Mathf.Clamp(currentFOV + amount, 0, 180), duration).SetEase(easeIn).OnComplete(() => {
                if (snapBack) {
                    firstPersonCamera.m_Lens.FieldOfView = currentFOV;
                } else {
                    DOTween.To(() => firstPersonCamera.m_Lens.FieldOfView, x => firstPersonCamera.m_Lens.FieldOfView = x,
                        currentFOV, duration).SetEase(easeOut);
                }
            });
    }
}
