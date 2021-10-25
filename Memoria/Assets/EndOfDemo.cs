using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDemo : MonoBehaviour {
    private bool done = false;
    void Update() {
        if (done) return;
        if (Vector3.Distance(transform.position, Globals.Player.transform.position) < 2) {
            done = true;
            string localized = Globals.Localization.Get("DEMO_FINISHED");
            Globals.UIManager.NotificationManager.NotifyPlayerBig(localized, () => { });
        }
    }
}