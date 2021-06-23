using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncePlayer : MonoBehaviour {
    private static Vector2 upNormal = new Vector2(0.0f, -1.0f);

    public float launchForce;
    public Vector3 punch;
    public float duration = 0.5f, elasticity = 0.3f;
    public int vibrato = 1;
    public ParticleSystem dust;

    private Tween tween;
    private Utils.Cooldown cooldown;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.contactCount > 0 && collision.relativeVelocity.y < -3.0f) {
                if (cooldown.Ready(0.1f) && Vector2.Dot(collision.contacts[0].normal, upNormal) > 0.9f) {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * launchForce, ForceMode2D.Impulse);
                    tween?.Kill(true);
                    tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);
                    dust.Play();
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFXChasingLevel/TifaJump", transform.position);
                }
            }
        }
    }
}
