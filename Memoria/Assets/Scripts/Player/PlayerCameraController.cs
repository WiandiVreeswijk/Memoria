using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerCameraController : MonoBehaviour {
    //3D
    public CinemachineVirtualCamera firstPersonCamera;

    //2D
    public CinemachineVirtualCamera cam;
    public GameObject cameraTarget;

    public void SetCameraTargetPosition(Vector3 position) {
        cameraTarget.transform.position = position;
    }

    public void ToggleCamera()
    {
        HouseCamera(firstPersonCamera.Priority != 11);
    }
    public void HouseCamera(bool enter)
    {
        if (enter)
        {
            Globals.Player.PlayerMovementAdventure.inHouse = true;
            firstPersonCamera.Priority = 11;
            Globals.Player.VisualEffects.SetMeshEnabled(false);
        }
        else
        {
            Globals.Player.PlayerMovementAdventure.inHouse = false;
            firstPersonCamera.Priority = 9;
            Globals.Player.VisualEffects.SetMeshEnabled(true);
        }
    }
}
