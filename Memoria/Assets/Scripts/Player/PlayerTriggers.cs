using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    public GameObject prefab;

    private CollectiblesCount cc;

    private void Start()
    {
        cc = FindObjectOfType<CollectiblesCount>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cc.leftTocollect = cc.leftTocollect - 1;
            Instantiate(prefab, transform.position, Quaternion.Euler(-90,0,0));
            Destroy(this.gameObject);
        }
    }
}
