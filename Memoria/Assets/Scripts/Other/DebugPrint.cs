using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugPrint : MonoBehaviour {
    private class DebugText {
        public string text;
        public float duration;
        public bool hasDuration;
    }

    public TextMeshProUGUI debugText;
    private Dictionary<string, DebugText> debugPrint = new Dictionary<string, DebugText>();

    void FixedUpdate() {
        string toPrint = "";
        debugPrint.RemoveAll((key, dText) => {
            toPrint += dText.text + "\n";
            if (dText.hasDuration) {
                dText.duration -= Time.fixedDeltaTime;
                if (dText.duration < 0) return true;
            } else return true;
            return false;
        });
        debugText.text = toPrint;
    }

    public void Print(string keyword, string text, float duration = -1) {
        debugPrint.TryGetValue(keyword, out DebugText dText);
        if (dText == null) dText = new DebugText();
        debugPrint[keyword] = dText;
        dText.text = text;
        dText.duration = duration;
        if (duration == -1) dText.hasDuration = false;
        else {
            dText.hasDuration = true;
            dText.duration = duration;
        }
    }

    public void SetTextVisible(bool toggle) {
        debugText.gameObject.SetActive(toggle);
    }
}
