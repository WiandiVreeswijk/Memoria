using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBuoyancy : MonoBehaviour {
    public float bobbingMagnitude = 0.0025f;
    private float randomInterval;
    private float randomInterval2;
    private Vector3 rot;

    public float speed = 0.5f;
    public float size = 0.5f;

    void Start()
    {
        rot = transform.eulerAngles;
        randomInterval = Random.Range(1f, 2f);
        randomInterval2 = Random.Range(1f, 2f);
    }

    void Update() {
        transform.position += new Vector3(0.0f, (Mathf.Cos(Time.time * randomInterval) * bobbingMagnitude), 0.0f);
        transform.eulerAngles = rot + new Vector3((Mathf.PerlinNoise(Time.time * speed + randomInterval, Time.time * speed * randomInterval2) - 0.5f) * size, 0.0f, (Mathf.PerlinNoise(Time.time * speed + randomInterval2, Time.time * speed * randomInterval) - 0.5f) * size);
    }
}
