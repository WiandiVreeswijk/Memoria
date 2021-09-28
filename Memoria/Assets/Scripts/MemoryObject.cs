using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MemoryObject : MonoBehaviour {
    public abstract void Activate();
    public abstract void UpdateDistance(float distance);
}
