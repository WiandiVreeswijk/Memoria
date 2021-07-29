using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundInteractable : iInteractable {
    [EventRef] public string soundEffect;

    void Start() {
        Resources.Load("Interactable/InteractableIconActiveMaterial");
        Resources.Load("Interactable/InteractableIconMaterial");
    }

    void Update() {

    }

    public override void OnInteract() {

    }

    public override void OnLookAt() {

    }

    public override void OnStopLookAt() {

    }
}
