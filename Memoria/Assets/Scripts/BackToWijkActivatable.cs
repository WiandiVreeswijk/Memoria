using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToWijkActivatable : MonoBehaviour, IActivatable {
    public void Activate() {
        Globals.Persistence.LeaveMemory();
    }
}
