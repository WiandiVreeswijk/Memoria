using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMemoryInteractable : MonoBehaviour, iInteractable {
    public void Interact() {
        Globals.Persistence.EnterMemory();
    }
}
