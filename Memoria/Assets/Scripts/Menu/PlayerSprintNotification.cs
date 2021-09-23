using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNotification : MonoBehaviour {
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Globals.UIManager.NotificationManager.NotifyPlayer("Hold SHIFT to walk", 4.5f);
            Destroy(gameObject);
        }
    }
}
