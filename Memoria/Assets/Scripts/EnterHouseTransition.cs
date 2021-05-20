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

    private void Start()
    {
        adventureMovement = FindObjectOfType<PlayerMovementAdventure>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!adventureMovement.inHouse)
        {
            if (other.gameObject.tag == "Player")
            {
                adventureMovement.inHouse = true;
                firstPersonCamera.Priority = 11;
            }
        }
        else
        {
            adventureMovement.inHouse = false;
            firstPersonCamera.Priority = 9;
        }
        elenaMesh.SetActive(!adventureMovement.inHouse);
    }
}
