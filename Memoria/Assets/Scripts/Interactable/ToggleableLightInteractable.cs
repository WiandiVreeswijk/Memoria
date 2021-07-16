using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ToggleableLightInteractable : MonoBehaviour, iInteractable {
    [EventRef] public string soundEffect;

    public new Light light;

    public void OnInteract() {
        //#Todo play click sound?
        FMODUnity.RuntimeManager.PlayOneShot(soundEffect);
        light.enabled ^= true;
    }

    public void OnLookAt() { }
    public void OnStopLookAt() { }
}
