using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {
    private TextMeshProUGUI textElement;
    private Image image;
    private string text;

    void Awake() {
        textElement = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void Set(string text) {
        this.text = text;
        textElement.text = text;
        RescaleWithText();
    }

    private void RescaleWithText() {
        RectTransform rt = textElement.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textElement.preferredWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textElement.preferredHeight);
    }

    void Update() {
        Color color = image.color;
        color.a = 0.5f + Mathf.Sin(Time.time * 2.0f) * 0.05f;
        image.color = color;
    }

    public string GetText() {
        return text;
    }
}
