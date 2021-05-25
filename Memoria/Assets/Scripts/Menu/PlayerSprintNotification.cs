using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNotification : MonoBehaviour {
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Globals.GetMenuController().NotifyPlayer("SHIFT to sprint", 2.5f);
            Destroy(gameObject);
        }
    }
}
