using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InteractableVisual : MonoBehaviour, IEnterActivatable, IExitActivatable {
    private IInteractable interactable;
    private GameObject iconObject;
    private MeshRenderer iconRenderer;

    void Start() {
        interactable = GetComponent<IInteractable>();
        Vector3 rotatedOffset = transform.rotation * interactable.iconOffset;
        iconObject = Instantiate(Globals.InteractableManager.iconPrefab, transform.position + rotatedOffset, Quaternion.identity);
        iconRenderer = iconObject.GetComponentInChildren<MeshRenderer>();
        iconObject.transform.parent = transform;
        SetRadiusAndCenter();
    }

    public void ActivateEnter() {
        iconRenderer.material = Globals.InteractableManager.iconActivated;
    }

    public void ActivateExit() {
        iconRenderer.material = Globals.InteractableManager.icon;
    }

    public void SetRadiusAndCenter() {
        SphereCollider coll = iconObject.GetComponent<SphereCollider>();
        Vector3 rotatedOffset = transform.rotation * interactable.iconOffset;
        coll.center = -rotatedOffset;
        coll.radius = interactable.interactionRadius;
    }
}
