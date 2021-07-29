using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iInteractable : MonoBehaviour {
    public Vector3 iconPosition;
    public abstract void OnInteract();
    public abstract void OnLookAt();
    public abstract void OnStopLookAt();
}
