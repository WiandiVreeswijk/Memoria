using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour {
    public TMPro.TextMeshProUGUI text;

    public void Open(string txt) {
        text.text = txt;
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
