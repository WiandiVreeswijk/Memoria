using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectiblesCount : MonoBehaviour
{
    public TextMeshProUGUI collectibleCount;

    private GameObject[] amountOfCollectibles;

    [HideInInspector]
    public int leftTocollect;
    void Start()
    {
        amountOfCollectibles = GameObject.FindGameObjectsWithTag("Collectible");
        leftTocollect = amountOfCollectibles.Length;
        collectibleCount.text = leftTocollect.ToString();
    }

    private void FixedUpdate()
    {
        collectibleCount.text = leftTocollect.ToString();
    }
}
