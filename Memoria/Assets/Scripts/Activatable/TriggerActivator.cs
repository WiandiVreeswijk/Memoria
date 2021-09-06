using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerActivatorType {
    ENTER,
    EXIT
}

public class TriggerActivator : MonoBehaviour {
    [Tooltip("Name of the trigger. Probably 'Player'")]
    public string triggerName;

    [Tooltip("Are the activatables attached to this object?")]
    public bool thisObject = false;
    [Tooltip("Are the activatables attached to this parent's object?")]
    public bool parentObject = false;

    [Tooltip("The activatable object that should be triggered")]
    public GameObject[] activatables;

    private List<T> GetActivatables<T>() {
        List<T> activatablesList = new List<T>();
        if (thisObject) activatablesList.AddRange(gameObject.GetComponents<T>());
        if (parentObject && transform.parent != null) activatablesList.AddRange(transform.parent.GetComponents<T>());
        foreach (var act in activatables) activatablesList.AddRange(act.GetComponents<T>());
        return activatablesList;
    }

    //#Todo Allow for multiple activatables connected to trigger activator (including self)
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(triggerName)) {
            foreach (var activatable in GetActivatables<IEnterActivatable>()) activatable.ActivateEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag(triggerName)) {
            foreach (var activatable in GetActivatables<IExitActivatable>()) activatable.ActivateExit();
        }
    }

    private void OnTriggerEnter(Collider collision) {
        print(collision.name);
        if (collision.CompareTag(triggerName)) {
            foreach (var activatable in GetActivatables<IEnterActivatable>()) activatable.ActivateEnter();
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.CompareTag(triggerName)) {
            foreach (var activatable in GetActivatables<IExitActivatable>()) activatable.ActivateExit();
        }
    }
}
