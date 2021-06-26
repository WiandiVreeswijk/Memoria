using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {
    public GameObject[] clouds;
    public Transform cloudRemovePoint;
    public float cloudSpeed = 0.1f;
    public float timeBetweenClouds = 2.0f;
    private float maxCloudTravelDistance;
    private Utils.Cooldown cooldown;

    void Start() {
        maxCloudTravelDistance = cloudRemovePoint.position.x - transform.position.x;
        if (clouds.Length == 0) {
            gameObject.SetActive(false);
            throw new System.Exception("No clouds assigned to cloud spawner!");
        } else Prewarm();
    }
    void FixedUpdate() {
        if (cooldown.Ready(2.0f)) {
            SpawnCloud(maxCloudTravelDistance);
        }
    }

    void SpawnCloud(float maxTravelDistance, float distanceFromSpawn = 0.0f) {
        GameObject randomPrefab = clouds[Random.Range(0, clouds.Length)];
        float y = Random.Range(-20.0f, 20.0f);
        float z = Random.Range(-40.0f, 40.0f);
        GameObject cloud = Instantiate(randomPrefab, transform.position + new Vector3(distanceFromSpawn, y, z), Quaternion.identity);
        cloud.transform.localScale = new Vector3(5f, 5f, 5f);
        Cloud cloudComponent = cloud.AddComponent<Cloud>();
        cloudComponent.Setup(maxTravelDistance - distanceFromSpawn, cloudSpeed);
    }

    void Prewarm() {
        float distance = 0;
        while (distance < maxCloudTravelDistance) {
            SpawnCloud(maxCloudTravelDistance, distance);
            distance += timeBetweenClouds * cloudSpeed;
        }
    }
}
