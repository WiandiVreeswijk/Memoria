using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Cinemachine;

public class EnterHouseTransition : MonoBehaviour
{
    public GameObject elenaMesh;

    private bool playerEntered = false;

    public CinemachineVirtualCamera firstPersonCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (!playerEntered)
        {
            if (other.gameObject.tag == "Player")
            {
                playerEntered = true;
                firstPersonCamera.Priority = 11;
            }
        }
        else
        {
            playerEntered = false;
            firstPersonCamera.Priority = 9;
        }
        elenaMesh.SetActive(!playerEntered);
    }
}
