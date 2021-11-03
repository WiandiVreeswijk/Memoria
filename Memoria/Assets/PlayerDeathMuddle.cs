using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathMuddle : MonoBehaviour {
    void FixedUpdate() {
        Globals.Screenshake.rumble = 0f;
    }
}
