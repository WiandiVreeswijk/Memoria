using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;

public class DeleteBus : MonoBehaviour, IEnterActivatable
{
    public GameObject cam;
    public void ActivateEnter() {
        Destroy(cam);
        Destroy(gameObject);
    }
}
