using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorPoint : MonoBehaviour {
    private void OnDrawGizmosSelected() {
        DrawArrow.ForGizmo(transform.position, transform.rotation * Vector3.forward / 2, 0.5f, 45f);
    }
}
