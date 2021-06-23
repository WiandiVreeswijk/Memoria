using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour {
    public float bounceBack;
    private float stunDuration = 0.5f;
    private PlayerMovement25D player;

    private void Start() {
        player = FindObjectOfType<PlayerMovement25D>();
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.contactCount > 0) {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                player.Stunned(stunDuration);

                //Bounce away from the fire
                float bounceForce = collision.relativeVelocity.x < 0 ? bounceBack : -bounceBack;
                if (collision.contacts[0].normal.y < -0.1f) { //If landing on top of the fire, remove a bit more vertical velocity.
                    bounceForce -= collision.relativeVelocity.x / 2.0f;
                }

                rb.AddForce(new Vector2(0, bounceBack * 5), ForceMode2D.Impulse);
                Utils.DelayedAction(0.1f, () => { //Delayed action so the player doesn't touch the ground when being launched backwards.
                    rb.AddForce(new Vector2(bounceForce, 0), ForceMode2D.Impulse);
                });
            }
        }
    }
}
