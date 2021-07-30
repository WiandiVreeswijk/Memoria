using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMemoryInteractable : IInteractable {
    public override void OnInteract() {
        Globals.Persistence.EnterMemory();
    }

    public override void OnLookAt() { }
    public override void OnStopLookAt() { }
}
