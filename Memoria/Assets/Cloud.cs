using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
    private float speed = 0;
    private float maxDistance = -1;
    private float startX;

    private void Start() {
        startX = transform.position.x;
    }

    void FixedUpdate() {
        transform.position += new Vector3(speed * Time.fixedDeltaTime, 0.0f, 0.0f);
        if (transform.position.x - startX > maxDistance) Destroy(gameObject);
        if (maxDistance == -1f) {
            Destroy(gameObject);
            throw new System.MissingMemberException("Max distance has not been set for cloud!");
        }
    }

    public void Setup(float maxTravelDistance, float cloudSpeed) {
        maxDistance = maxTravelDistance;
        speed = cloudSpeed;
    }
}
