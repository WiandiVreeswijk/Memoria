using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] clouds;
    public float maxCloudTravelDistance = 10;
    private Utils.Cooldown cooldown;

    void FixedUpdate()
    {
        if (clouds.Length > 0)
        {
            if (cooldown.Ready(10.0f))
            {
                GameObject randomPrefab = clouds[Random.Range(0, clouds.Length)];
                GameObject cloud = Instantiate(randomPrefab, transform.position + new Vector3(0.0f, Random.Range(-20.0f, 20.0f), Random.Range(-40.0f, 40.0f)), Quaternion.identity);
                cloud.transform.localScale = new Vector3(10f, 10f, 10f);
                Cloud cloudComponent = cloud.AddComponent<Cloud>();
                cloudComponent.SetMaxDistance(maxCloudTravelDistance);
            }
        }
    }
}
