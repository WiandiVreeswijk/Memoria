using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffsetBasedOnCamera : MonoBehaviour {
    void LateUpdate() {
        transform.position = Globals.Player.transform.position + Camera.main.transform.right * 1;
        print(transform.position);
    }
}
