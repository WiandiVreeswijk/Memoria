using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CloudSpawner : MonoBehaviour {
    public GameObject[] clouds;
    public Bounds spawnArea = new Bounds(Vector3.zero, Vector3.one);
    public float cloudSpeed = 0.1f;
    [Range(0.1f, 100f)] public float minTimeBetweenClouds = 2.0f;
    [Range(0.1f, 100f)] public float maxTimeBetweenClouds = 4.0f;
    public float minSize = 5.0f;
    public float maxSize = 10.0f;

    private bool runInEditor = false;
    private float nextInterval;
    private Utils.Cooldown cooldown;

    void Start() {
        Cleanup();
        GetNextInterval();
        if (clouds.Length == 0) {
            gameObject.SetActive(false);
            throw new System.Exception("No clouds assigned to cloud spawner!");
        } else if (Application.isPlaying || runInEditor) Prewarm();
    }

    void Update() {
        if (Application.isPlaying || runInEditor) {
            if (cooldown.Ready(nextInterval)) {
                SpawnCloud(spawnArea.size.x);
                nextInterval = GetNextInterval();
            }
        }
    }

    void SpawnCloud(float maxTravelDistance, float distanceFromSpawn = 0.0f) {
        GameObject randomPrefab = clouds[Random.Range(0, clouds.Length)];
        float y = Random.Range(spawnArea.center.y - spawnArea.size.y / 2, spawnArea.center.y + spawnArea.size.y / 2);
        float z = Random.Range(spawnArea.center.z - spawnArea.size.z / 2, spawnArea.center.z + spawnArea.size.z / 2);
        GameObject cloud = Instantiate(randomPrefab, transform.position + new Vector3(distanceFromSpawn, y, z), Quaternion.Euler(0.0f, Random.Range(0f, 360f), 0.0f));
        cloud.transform.parent = transform;
        float size = Random.Range(minSize, maxSize);
        cloud.transform.localScale = new Vector3(size, size, size);
        Cloud cloudComponent = cloud.AddComponent<Cloud>();
        cloudComponent.Setup(maxTravelDistance - distanceFromSpawn, Random.Range(0.5f * cloudSpeed, 1.5f * cloudSpeed));
    }

    float GetNextInterval() {
        return Random.Range(minTimeBetweenClouds, maxTimeBetweenClouds);
    }

    void Prewarm() {
        float distance = 0;
        while (distance < spawnArea.size.x) {
            SpawnCloud(spawnArea.size.x, distance);
            if (cloudSpeed <= 0.0f) cloudSpeed += 0.01f;
            distance += GetNextInterval() * cloudSpeed;
        }
    }

    public bool IsRunningInEditor() {
        return runInEditor;
    }

    public void Toggle() {
        if (runInEditor) {
            Cleanup();
        } else {
            Prewarm();
        }
        runInEditor ^= true;
    }

    private void Cleanup() {
        while (transform.childCount > 0) {
            GameObject child = transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }
    }

    public void Reset() {
        if (Application.isPlaying || runInEditor) {
            Cleanup();
            Prewarm();
        }
    }
}
