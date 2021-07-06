using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour {
    private bool isActive = false;
    private DebugPrint debugPrint;

    /*Debug menu*/
    public CanvasGroup debugMenuPanel;
    private bool debugMenuVisible = false;

    private void Start() {
        debugPrint = GetComponent<DebugPrint>();
        isActive = Application.isEditor || Debug.isDebugBuild;
        gameObject.SetActive(isActive);
        SetVisible(false);
    }

    public void Print(string keyword, string text, float duration = -1)
    {
        if (!isActive) return;
        debugPrint.Print(keyword, text, duration);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            SetVisible(!debugMenuVisible);
        }
    }

    private void SetVisible(bool toggle) {
        if (toggle) {
            debugMenuPanel.alpha = 1.0f;
            debugMenuPanel.blocksRaycasts = true;
            debugMenuPanel.interactable = true;
            debugMenuVisible = true;
            debugPrint.SetTextVisible(false);
        } else {
            debugMenuPanel.alpha = 0.0f;
            debugMenuPanel.blocksRaycasts = false;
            debugMenuPanel.interactable = false;
            debugMenuVisible = false;
            debugPrint.SetTextVisible(true);
        }
    }
}