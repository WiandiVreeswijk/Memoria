using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActiveCamera : MonoBehaviour
{
    public Vector3 offset;
    void FixedUpdate() {
        transform.position = Camera.main.transform.position + offset;
    }
}
