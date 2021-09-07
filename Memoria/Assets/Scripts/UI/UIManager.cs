using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] private Canvas overlayCanvas;
    public Canvas OverlayCanvas => overlayCanvas;

    [SerializeField] private Canvas screenspaceCanvas;
    public Canvas ScreenspaceCanvas => screenspaceCanvas;

    private ChasingLevelUI chasingLevelUI;
    public ChasingLevelUI ChasingLevel => chasingLevelUI;

    public void OnGlobalsInitialize() {
        chasingLevelUI = GetComponentInChildren<ChasingLevelUI>();
        screenspaceCanvas.worldCamera = Globals.Camera;
    }
}
