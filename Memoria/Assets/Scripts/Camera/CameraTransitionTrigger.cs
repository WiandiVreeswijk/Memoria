using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransitionTrigger : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (camera.Priority == 0)
            {
                camera.Priority = 2;
            }
            else
            {
                camera.Priority = 0;
            }
        }
    }
}
