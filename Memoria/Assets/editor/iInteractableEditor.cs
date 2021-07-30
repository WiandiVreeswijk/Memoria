using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IInteractable), true)]
[CanEditMultipleObjects]
public class iInteractableEditor : Editor {
    public override void OnInspectorGUI() {
        IInteractable interactable = target as IInteractable;
        base.OnInspectorGUI();
    }

    public void OnSceneGUI() {
        IInteractable interactable = target as IInteractable;

        Vector3 rotatedPos = interactable.transform.rotation * interactable.iconOffset;
        Vector3 newPos = Handles.FreeMoveHandle(rotatedPos + interactable.transform.position, Quaternion.identity, 0.15f, new Vector3(0.25f, 0.25f, 0), Handles.SphereHandleCap) - interactable.transform.position;
        if (newPos != rotatedPos) {
            interactable.iconOffset = Quaternion.Inverse(interactable.transform.rotation) * newPos;
            EditorUtility.SetDirty(interactable);
        }

    }
}
