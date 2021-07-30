using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ToggleableLightInteractable : IInteractable {
    [EventRef] public string soundEffect;

    public new Light light;

    public override void OnInteract() {
        //#Todo play click sound?
        FMODUnity.RuntimeManager.PlayOneShot(soundEffect);
        light.enabled ^= true;
    }

    public override void OnLookAt() { }
    public override void OnStopLookAt() { }
}
