using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class GemCollectionActivator : MonoBehaviour, IActivatable {
    [EventRef]
    public string soundEffect = "";

    public void Activate() {
        Destroy(gameObject);
        FMODUnity.RuntimeManager.PlayOneShot(soundEffect, transform.position);
    }
}
