using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableVisual))]
public abstract class IInteractable : MonoBehaviour {
    public Vector3 iconOffset;
    public float interactionRadius = 15.0f;
    public abstract void OnInteract();
    public abstract void OnLookAt();
    public abstract void OnStopLookAt();

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
