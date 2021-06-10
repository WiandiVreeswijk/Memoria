using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour {
    public string triggerName;
    public GameObject activatable;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(triggerName)) {
            var activatables = activatable.GetComponents<IActivatable>();
            foreach (var activatable in activatables) {
                activatable.Activate();
            }
        }
    }
}
