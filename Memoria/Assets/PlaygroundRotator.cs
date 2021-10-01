using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundRotator : MonoBehaviour {
    void FixedUpdate() {
        transform.Rotate(new Vector3(0f, 0.02f, 0f));
    }
}
