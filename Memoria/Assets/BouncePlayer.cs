using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncePlayer : MonoBehaviour
{
    public float launchForce;

    public Vector3 punch;
    public float duration = 0.5f, elasticity = 0.3f;
    public int vibrato = 1;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * launchForce;
            transform.DOPunchScale(punch, duration, vibrato, elasticity);
        }
    }
}
