using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour {
    public TMPro.TextMeshProUGUI text;
    private bool open = false;


    private void Start() {
        gameObject.SetActive(false);
    }

    public void Open(string txt) {
        text.text = txt;
        gameObject.SetActive(true);
        open = true;
    }

    public void Close() {
        gameObject.SetActive(false);
        open = false;
        Globals.CursorManager.LockMouse();
        Globals.CinemachineManager.SetInputEnabled(true);
        if (Globals.GetCurrentGlobalsType() == Globals.GlobalsType.NEIGHBORHOOD) Globals.Player.PlayerMovementAdventure.SetCanMove(true);
    }

    public void FixedUpdate() {
        if (open) {
            Globals.CursorManager.UnlockMouse();
            Globals.CinemachineManager.SetInputEnabled(false);
            if (Globals.GetCurrentGlobalsType() == Globals.GlobalsType.NEIGHBORHOOD) Globals.Player.PlayerMovementAdventure.SetCanMove(false);
        }
    }
}
