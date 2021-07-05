using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour {
    private DebugPrint debugPrint;

    private void Start() {
        debugPrint = GetComponent<DebugPrint>();
    }

    public void Print(string keyword, string text, float duration = -1) {
        debugPrint.Print(keyword, text, duration);
    }
}
