using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OblivionSafeArea : MonoBehaviour {
    public Transform oblivionStopPosition;
    public Transform oblivionContinuePosition;
    public Rigidbody2D blockingWall;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            blockingWall.simulated = true;
        }
    }
}
