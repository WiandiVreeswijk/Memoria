using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] private Canvas screenspaceCanvas;
    public Canvas ScreenspaceCanvas => screenspaceCanvas;

    void Start() {
        screenspaceCanvas.worldCamera = Camera.main;
    }
}
