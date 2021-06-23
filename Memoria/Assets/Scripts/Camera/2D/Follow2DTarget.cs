using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow2DTarget : MonoBehaviour {
    public float speed = 2.0f;

    void FixedUpdate() {
        float interpolation = speed * Time.fixedDeltaTime;

        Vector3 position = transform.position;
        position.y = Mathf.Lerp(transform.position.y, Globals.Player.transform.position.y, interpolation);
        position.x = Mathf.Lerp(transform.position.x, Globals.Player.transform.position.x, interpolation);
        transform.position = position;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawLine(transform.position, Globals.Player.transform.position);
    }
}
