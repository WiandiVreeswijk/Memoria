using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class GemCollectionActivator : MonoBehaviour, IActivatable {
    [EventRef]
    public string soundEffect = "";

    public void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.Play("GemCollectible", 0, Mathf.Sin((transform.position.x % 5.0f) / 5.0f));
    }
    public void Activate() {
        Destroy(gameObject);
        FMODUnity.RuntimeManager.PlayOneShot(soundEffect, transform.position);
    }
}
