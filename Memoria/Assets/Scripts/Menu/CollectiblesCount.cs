using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectiblesCount : MonoBehaviour {
    public TextMeshProUGUI collectibleCount;

    private GameObject[] amountOfCollectibles;

    private FireworksSpawner spawner;
    [HideInInspector]
    public int leftTocollect;
    void Start() {
        spawner = FindObjectOfType<FireworksSpawner>();
        amountOfCollectibles = GameObject.FindGameObjectsWithTag("Collectible");
        leftTocollect = amountOfCollectibles.Length;
        collectibleCount.text = $"{leftTocollect}";
    }

    private void FixedUpdate() {
        collectibleCount.text = $"{leftTocollect}";
        if (leftTocollect == 0) {
            spawner.SetActive();
        }
    }
}
