using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour {
    [Tooltip("Name of the trigger. Probably 'Player'")]
    public string triggerName;

    [Tooltip("Are the activatables attached to this object?")]
    public bool activatablesAttachedToThisObject = false;

    [Tooltip("The activatable object that should be triggered")]
    public GameObject activatable;

    //#Todo Allow for multiple activatables connected to trigger activator (including self)
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(triggerName)) {
            GameObject obj = activatablesAttachedToThisObject ? gameObject : activatable;
            var activatables = obj.GetComponents<IActivatable>();
            foreach (var activatable in activatables) {
                activatable.Activate();
            }
        }
    }
}
