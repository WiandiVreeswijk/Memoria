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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * launchForce; 
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * launchForce, ForceMode2D.Impulse);
            if (tween != null) tween.Kill(true);
            tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);
        }
    }
}
