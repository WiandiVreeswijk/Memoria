using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour {
    public TMPro.TextMeshProUGUI text;
    public GameObject gemImage;

    private bool open = false;


    private void Start() {
        gameObject.SetActive(false);
    }

    private bool CheckKeyword(ref string content, string keyword) {
        bool startsWith = content.StartsWith(keyword);
        if (startsWith) {
            content = content.Substring(keyword.Length);
        }

        return startsWith;

    }

    public void Open(string content) {
        gemImage.SetActive(CheckKeyword(ref content, "[GEM]"));
        text.text = content;
        gameObject.SetActive(true);
        open = true;
    }

    public void Close() {
        gameObject.SetActive(false);
        open = false;
        Globals.CursorManager.LockMouse();
        Globals.CinemachineManager.SetInputEnabled(true);
        if (Globals.GetCurrentGlobalsType() == Globals.GlobalsType.NEIGHBORHOOD) Globals.Player.PlayerMovementAdventure.SetCanMove(true);
        if (Globals.GetCurrentGlobalsType() == Globals.GlobalsType.OBLIVION) Globals.Player.PlayerMovement25D.SetStunned(false, false, false);
    }

    public void FixedUpdate() {
        if (open) {
            Globals.CursorManager.UnlockMouse();
            Globals.CinemachineManager.SetInputEnabled(false);
            if (Globals.GetCurrentGlobalsType() == Globals.GlobalsType.NEIGHBORHOOD) Globals.Player.PlayerMovementAdventure.SetCanMove(false);
            if (Globals.GetCurrentGlobalsType() == Globals.GlobalsType.OBLIVION) Globals.Player.PlayerMovement25D.SetStunned(true, true, true);
        }
    }
}
