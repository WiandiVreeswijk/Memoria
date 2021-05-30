using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Renderer checkpoint;
    public Color activateColor, inactiveColor;

    private PlayerRespawn respawn;

    private void Start()
    {
        respawn = FindObjectOfType<PlayerRespawn>();
        NotActivated();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Activated();
        }
    }

    private void Activated()
    {
        checkpoint.material.color = activateColor;
        respawn.playerStartPosition = transform.position;
    }

    private void NotActivated()
    {
        checkpoint.material.color = inactiveColor;
    }
}

