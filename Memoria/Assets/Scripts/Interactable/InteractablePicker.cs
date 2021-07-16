using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePicker : MonoBehaviour {
    //private bool ghostObjectCreated = false;
    private GameObject ghostObject;
    private GameObject currentlyHoveredObject;
    private GameObject previouslyHoveredObject;

    private iInteractable hoveredInteractable;
    private MeshRenderer hoveredRenderer;
    private Material oldMaterial;
    public Material hoverMaterial;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && currentlyHoveredObject != null) {
            iInteractable interactable = currentlyHoveredObject.GetComponent<iInteractable>();
            if (interactable == null) throw new System.Exception($"No interactable found on hovered object {currentlyHoveredObject.name}");
            interactable.OnInteract();
        }
    }

    void FixedUpdate() {
        Ray ray = Globals.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        GameObject hoveredObject = null;
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Transform objectHit = hit.transform;
            hoveredObject = objectHit.gameObject;
        }

        if (currentlyHoveredObject != null && hoveredObject != currentlyHoveredObject) {
            hoveredRenderer.sharedMaterial = oldMaterial;
            currentlyHoveredObject = null;
            hoveredInteractable.OnLookAt();
        }

        if (hoveredObject != null)
        {
            hoveredInteractable = hoveredObject.GetComponent<iInteractable>();
            if (currentlyHoveredObject != hoveredObject && hoveredInteractable != null) {
                currentlyHoveredObject = hoveredObject;
                hoveredRenderer = currentlyHoveredObject.GetComponent<MeshRenderer>();
                oldMaterial = hoveredRenderer.sharedMaterial;
                hoverMaterial.SetTexture("_BaseMap", oldMaterial.GetTexture("_MainTex"));
                hoveredRenderer.sharedMaterial = hoverMaterial;
                hoveredInteractable.OnStopLookAt();
            }
        }

        //previouslyHoveredObject = hoveredObject;
    }
}
