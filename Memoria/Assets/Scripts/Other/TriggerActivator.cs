using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour {
    [Tooltip("Name of the trigger. Probably 'Player'")]
    public string triggerName;

    [Tooltip("Are the activatables attached to this object?")]
    public bool activatablesAttachedToThisObject = false;

    [Tooltip("The activatable object that should be triggered")]
    public GameObject[] activatables;

    //#Todo Allow for multiple activatables connected to trigger activator (including self)
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(triggerName)) {
            List<IEnterActivatable> activatablesList = new List<IEnterActivatable>();
            if (activatablesAttachedToThisObject) activatablesList.AddRange(gameObject.GetComponents<IEnterActivatable>());
            foreach (var act in activatables) activatablesList.AddRange(act.GetComponents<IEnterActivatable>());
            foreach (var activatable in activatablesList) activatable.ActivateEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag(triggerName)) {
            List<IExitActivatable> activatablesList = new List<IExitActivatable>();
            if (activatablesAttachedToThisObject) activatablesList.AddRange(gameObject.GetComponents<IExitActivatable>());
            foreach (var act in activatables) activatablesList.AddRange(act.GetComponents<IExitActivatable>());
            foreach (var activatable in activatablesList) activatable.ActivateExit();
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.CompareTag(triggerName)) {
            List<IEnterActivatable> activatablesList = new List<IEnterActivatable>();
            if (activatablesAttachedToThisObject) activatablesList.AddRange(gameObject.GetComponents<IEnterActivatable>());
            foreach (var act in activatables) activatablesList.AddRange(act.GetComponents<IEnterActivatable>());
            foreach (var activatable in activatablesList) activatable.ActivateEnter();
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.CompareTag(triggerName)) {
            List<IExitActivatable> activatablesList = new List<IExitActivatable>();
            if (activatablesAttachedToThisObject) activatablesList.AddRange(gameObject.GetComponents<IExitActivatable>());
            foreach (var act in activatables) activatablesList.AddRange(act.GetComponents<IExitActivatable>());
            foreach (var activatable in activatablesList) activatable.ActivateExit();
        }
    }
}
