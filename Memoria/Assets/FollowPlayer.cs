using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject toFollow;

    void Update() {
        transform.position = Vector3.Lerp(transform.position, toFollow.transform.position + new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * 5);
    }
}
