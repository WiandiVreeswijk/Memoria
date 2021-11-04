using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class ControllerFastForward : MonoBehaviour {
    private StandardUIContinueButtonFastForward ff;
    // Start is called before the first frame update
    void Start() {
        ff = GetComponent<StandardUIContinueButtonFastForward>();
    }

    // Update is called once per frame
    void Update() {
        //if (Hinput.anyGamepad.B.justPressed)
        //{
        //    print("test");
        //    ff.OnFastForward();
        //}
    }
}
