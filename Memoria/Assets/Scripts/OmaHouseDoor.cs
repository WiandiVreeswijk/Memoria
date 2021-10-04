using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmaHouseDoor : MonoBehaviour {
    void Awake() {
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void OpenDoor() {
        transform.localRotation = Quaternion.Euler(0.0f, -115f, 0.0f);
    }
}
