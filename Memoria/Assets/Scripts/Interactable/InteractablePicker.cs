using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePicker : MonoBehaviour {
    private bool ghostObjectCreated = false;
    private GameObject ghostObject;
    private GameObject currentlyHoveredObject;
    private GameObject previouslyHoveredObject;
    public Material hoverMaterial;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && currentlyHoveredObject != null) {
            iInteractable interactable = currentlyHoveredObject.GetComponent<iInteractable>();
            if (interactable == null) throw new System.Exception($"No interactable found on hovered object {currentlyHoveredObject.name}");
            interactable.Interact();
        }
    }
    
    void FixedUpdate() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    
        GameObject hoveredObject = null;
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Transform objectHit = hit.transform;
            hoveredObject = objectHit.gameObject;
            if (objectHit.tag == "Interactable" && !ghostObjectCreated) {
                ghostObjectCreated = true;
                ghostObject = Instantiate(hoveredObject);
                ghostObject.transform.localScale = hoveredObject.transform.localScale * 1.05f;
                ghostObject.GetComponent<MeshRenderer>().material = hoverMaterial;
                ghostObject.GetComponent<Collider>().enabled = false;
                currentlyHoveredObject = hoveredObject;
            }
        }
    
        if (hoveredObject != previouslyHoveredObject && ghostObjectCreated) {
            Destroy(ghostObject);
            ghostObject = null;
            ghostObjectCreated = false;
            currentlyHoveredObject = null;
        }
    
        previouslyHoveredObject = hoveredObject;
    }
}
