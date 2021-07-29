using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(iInteractable), true)]
[CanEditMultipleObjects]
public class iInteractableEditor : Editor {
    public override void OnInspectorGUI() {
        iInteractable interactable = target as iInteractable;
        base.OnInspectorGUI();



        if (GUILayout.Button("test")) {
        }
    }

    public void OnSceneGUI() {
        iInteractable interactable = target as iInteractable;
        Vector3 newPos = Handles.FreeMoveHandle(interactable.iconPosition + interactable.transform.position, Quaternion.identity, 0.15f, new Vector3(0.25f, 0.25f, 0), Handles.SphereHandleCap) - interactable.transform.position;
        if (interactable.iconPosition != newPos) {
            interactable.iconPosition = newPos;
            EditorUtility.SetDirty(interactable);
        }

    }
}
