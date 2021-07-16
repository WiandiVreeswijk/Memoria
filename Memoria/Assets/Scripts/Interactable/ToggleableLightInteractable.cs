using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableLightInteractable : MonoBehaviour, iInteractable {
    public new Light light;

    public void Interact() {
        //#Todo play click sound?
        light.enabled ^= true;
    }
}
