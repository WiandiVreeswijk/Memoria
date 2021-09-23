using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStart : MonoBehaviour {
    public Transform position;
    void Start() {
        transform.position = position.position;
    }
}
