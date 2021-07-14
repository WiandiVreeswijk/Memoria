using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuggerButton : MonoBehaviour {
    private Action action;

    public void OnClick() {
        action();
    }

    public void Set(Action action, bool enabled, string text) {
        this.action = action;
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        GetComponentInChildren<UnityEngine.UI.Button>().interactable = enabled;
    }
}
