using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncePlayer : MonoBehaviour {
    public float launchForce;

    public Vector3 punch;
    public float duration = 0.5f, elasticity = 0.3f;
    public int vibrato = 1;
    private Tween tween;
    private Utils.Cooldown cooldown;

    private ParticleSystem dust;

    private void Start()
    {
        dust = gameObject.GetComponentInChildren<ParticleSystem>();
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (cooldown.Ready(0.1f)) {
                //collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * launchForce; 
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * launchForce, ForceMode2D.Impulse);
                tween?.Kill(true);
                tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);
                dust.Play();
            }
        }
    }
}
