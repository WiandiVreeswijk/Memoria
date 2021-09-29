using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {
    private Image image;
    private string text;

    void Start() {
        image = GetComponent<Image>();
    }

    public void Set(string text) {
        this.text = text;
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
