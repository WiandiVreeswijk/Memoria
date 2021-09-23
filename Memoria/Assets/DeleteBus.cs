using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bus"))
        {
            Destroy(other);
        }
    }
}
