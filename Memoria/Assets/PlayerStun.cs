using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    public float bounceBack;
    private float stunDuration = 0.5f;
    private PlayerMovement25D player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement25D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(-bounceBack, bounceBack), ForceMode2D.Impulse);
            player.Stunned(stunDuration);
        }
    }
}
