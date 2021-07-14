using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectibleVisual : MonoBehaviour {
    private float maxSpeed = 20.0f;
    private float minSpeed = 10.00f;
    private float maxSize = 1.0f;
    private float minSize = 0.075f;
    private float initialDistance;
    private float speed;
    Vector3 originalUIScale;

    void Start() {
        speed = maxSpeed;
        originalUIScale = transform.localScale;
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.975f, Camera.main.nearClipPlane));
        initialDistance = Vector3.Distance(transform.position, worldPosition);
    }

    void LateUpdate() {
        print(Camera.main);
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.975f, Camera.main.nearClipPlane));
        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * speed);
        float distance = Vector3.Distance(transform.position, worldPosition);
        if (distance < 0.05f) {
            Globals.ScoreManager.AddCollectible();
            Destroy(gameObject);
        }

        speed = Utils.Remap(distance, 0.0f, initialDistance, minSpeed, maxSpeed);
        float size = Utils.Remap(distance, 0.0f, initialDistance, minSize, maxSize);
        transform.localScale = size * originalUIScale;
    }

    //Called by animation but empty because we don't want flares on the UI collectibles.
    public void PlayPFX() { }
}
