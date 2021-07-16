using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMemoryInteractable : MonoBehaviour, iInteractable {
    public void OnInteract() {
        Globals.Persistence.EnterMemory();
    }

    public void OnLookAt() { }
    public void OnStopLookAt() { }
}
