using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectibleAnimationOffset : MonoBehaviour {
    public void Start() {
        Animator animator = GetComponentInChildren<Animator>();
        animator.Play("GemCollectible", 0, Mathf.Sin((transform.position.x % 5.0f) / 5.0f));
    }
}
