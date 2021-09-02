using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Cinemachine;

public class EnterHouseTransition : MonoBehaviour
{
    public GameObject elenaMesh;

    public CinemachineVirtualCamera firstPersonCamera;

    private PlayerMovementAdventure adventureMovement;

    private bool isColliding = false;

    private void Start()
    {
        adventureMovement = FindObjectOfType<PlayerMovementAdventure>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;

        print("entered house");
        if (other.gameObject.tag == "Player")
        {
            Globals.Player.CameraController.ChangeCamera(PlayerCameraController.CameraType.FIRSTPERSON);
        }
        elenaMesh.SetActive(false);

        StartCoroutine(Reset());

    }

    private void OnTriggerExit(Collider other)
    {
        print("exit house");
        if (other.gameObject.tag == "Player")
        {
            Globals.Player.CameraController.ChangeCamera(PlayerCameraController.CameraType.THIRDPERSON);
            firstPersonCamera.Priority = 9;
        }
        elenaMesh.SetActive(true);
    }

    IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }
}
