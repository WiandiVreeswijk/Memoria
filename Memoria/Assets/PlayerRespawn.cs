using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool respawnPlayer = false;

    private Vector3 playerPosition;
    private Vector3 playerStartPosition;


    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        playerStartPosition = playerPosition;
    }

    private void FixedUpdate()
    {
        if (respawnPlayer)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = playerStartPosition;
            respawnPlayer = false;
            print("respawned");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("triggered");
            respawnPlayer = true;
        }
    }
}
