using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private OblivionManager oblivion;
    private PlayerVisualEffects visuals;
    private float offset = 3;

    [HideInInspector]
    public Vector3 playerStartPosition;

    private bool respawnPlayer = false;

    private void Start()
    {
        oblivion = FindObjectOfType<OblivionManager>();
        visuals = FindObjectOfType<PlayerVisualEffects>();

        transform.position = playerStartPosition;
    }
    private void FixedUpdate()
    {
        if(oblivion.GetOblivionPosition() - offset > transform.position.x)
        {
            respawnPlayer = true;
            if (respawnPlayer)
            {
                visuals.Death();
                Invoke("Respawn",1f);
                respawnPlayer = false;
            }
        }
    }

    private void Respawn()
    {
        visuals.isDeath = false;
        transform.position = playerStartPosition;
        visuals.elenaMesh.enabled = true;
        oblivion.SetOblivionPosition(-5);
    }

}
