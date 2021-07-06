using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class GemCollectible : MonoBehaviour, IActivatable {
    [EventRef]
    public string soundEffect = "";
    private ParticleSystem pSystem;
    private bool collected = false;

    public void Start() {
        Animator animator = GetComponentInChildren<Animator>();
        animator.Play("GemAnimation", 0, ((transform.position.x % 10) / 5.0f) - 1.0f);
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    private Vector3 pos;
    public void Activate() {
        //Destroy(gameObject);
        GetComponent<Collider2D>().enabled = false;
        FMODUnity.RuntimeManager.PlayOneShot(soundEffect, transform.position);
        collected = true;
        //pos = transform.position - Camera.main.transform.position;
        transform.parent = Globals.GetInstance().canvas.transform;
        gameObject.layer = 5;
    }

    public void PlayPFX() {
        pSystem.Play();
    }

    void LateUpdate() {
        if (collected) {
            //Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.9f, Camera.main.nearClipPlane));
            //transform.position = Vector3.Lerp(transform.position + pos, position, Time.deltaTime * 2.5f);
            //float distance = Vector3.Distance(transform.position + pos, position);
            //if (distance > 15.0f) distance = 15.0f;
            //float a = distance;
            //distance = Utils.Remap(distance, 0.0f, 15.0f, 1.0f, 1.0f);
            //Globals.Debugger.Print("a", a + " + " + distance, 1.0f);
            //transform.localScale = new Vector3(distance, distance, distance);
            //if (distance < 0.2f) Destroy(gameObject);
        }
    }
}