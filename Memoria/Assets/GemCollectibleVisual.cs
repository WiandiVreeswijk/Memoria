using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectibleVisual : MonoBehaviour {
    private float maxSpeed = 20.0f;
    private float minSpeed = 10.00f;
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
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.975f, Camera.main.nearClipPlane));
        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * speed);
        float distance = Vector3.Distance(transform.position, worldPosition);
        if (distance < 0.1f) Destroy(gameObject);

        speed = Utils.Remap(distance, 0.0f, initialDistance, minSpeed, maxSpeed);
        distance = Utils.Remap(distance, 0.0f, initialDistance, 0.1f, 1.0f);
        transform.localScale = distance * originalUIScale;
    }

    //Called by animation but empty because we don't want flares on the UI collectibles.
    public void PlayPFX() { }
}
