using System.Collections;
using System.Collections.Generic;
using HinputClasses;
using UnityEngine;

public class ControllerManager : MonoBehaviour {
    bool isUsingController = false;
    bool justChanged = true;

    private void Start() {
        isUsingController = Hinput.anyGamepad.isConnected;
    }

    void LateUpdate() {
        justChanged = false;
        bool original = isUsingController;
        if (Input.anyKey) isUsingController = false;
        CheckGamepadButton(Hinput.anyGamepad.A);
        CheckGamepadButton(Hinput.anyGamepad.B);
        CheckGamepadButton(Hinput.anyGamepad.X);
        CheckGamepadButton(Hinput.anyGamepad.Y);
        CheckGamepadButton(Hinput.anyGamepad.leftBumper);
        CheckGamepadButton(Hinput.anyGamepad.rightBumper);
        CheckGamepadButton(Hinput.anyGamepad.leftTrigger);
        CheckGamepadButton(Hinput.anyGamepad.rightTrigger);
        CheckGamepadButton(Hinput.anyGamepad.back);
        CheckGamepadButton(Hinput.anyGamepad.start);
        CheckGamepadButton(Hinput.anyGamepad.leftStickClick);
        CheckGamepadButton(Hinput.anyGamepad.rightStickClick);
        CheckGamepadButton(Hinput.anyGamepad.dPad.left);
        CheckGamepadButton(Hinput.anyGamepad.dPad.right);
        CheckGamepadButton(Hinput.anyGamepad.dPad.up);
        CheckGamepadButton(Hinput.anyGamepad.dPad.down);
        if (isUsingController != original) justChanged = true;
    }

    private void CheckGamepadButton(Pressable pr) {
        if (Hinput.anyGamepad.isConnected && pr.pressed) {
            isUsingController = true;
        }
    }

    //public bool JustChanged() {
    //    return justChanged;
    //}

    public bool IsUsingController() {
        return isUsingController;
    }
}
