using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] private Canvas screenspaceCanvas;
    public Canvas ScreenspaceCanvas => screenspaceCanvas;

    private ChasingLevelUI chasingLevelUI;
    public ChasingLevelUI ChasingLevel => chasingLevelUI;

    void Start() {
        chasingLevelUI = GetComponentInChildren<ChasingLevelUI>();
        screenspaceCanvas.worldCamera = Camera.main;
    }
}
