using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MemoryObject : MonoBehaviour {
    [SerializeField]
    private UnityEvent onWatchUse;

    public void UseWatch() {
        onWatchUse.Invoke();
    }
}
