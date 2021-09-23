using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyObjectRotation : MonoBehaviour {
    public GameObject target;

    public bool update;
    public bool fixedUpdate;

    void Update() {
        if (update && target != null) transform.rotation = target.transform.rotation;
    }
    void FixedUpdate() {
        if (fixedUpdate && target != null) transform.rotation = target.transform.rotation;
    }
}
