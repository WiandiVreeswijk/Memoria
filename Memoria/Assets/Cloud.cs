using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cloud : MonoBehaviour {
    private float speed = 0;
    private float maxDistance = -1;
    private float startX;

    private void Start() {
        startX = transform.position.x;
    }

    void Update() {
        transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        if (transform.position.x - startX > maxDistance) {
            if (gameObject != null) {
                if (Application.isPlaying) Destroy(gameObject);
                else DestroyImmediate(gameObject);
            }
        }
    }

    public void Setup(float maxTravelDistance, float cloudSpeed) {
        maxDistance = maxTravelDistance;
        speed = cloudSpeed;
    }
}
