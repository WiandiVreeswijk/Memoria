using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTrigger : MonoBehaviour {
    public string progressTag;
    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            ProgressionManager.Progress(progressTag);
        }
    }
}
