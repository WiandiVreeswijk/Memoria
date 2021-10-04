using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    private CursorLockMode lockState = CursorLockMode.None;
    private bool visible = true;
    public void LockMouse() {
        lockState = CursorLockMode.Locked;
        visible = false;
    }
    public void UnlockMouse() {
        lockState = CursorLockMode.None;
        visible = true;
    }

    public void Update() {
        Cursor.lockState = lockState;
        Cursor.visible = visible;
    }
}
