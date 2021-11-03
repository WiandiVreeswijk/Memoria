using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonsUI : MonoBehaviour {
    public GameObject xbox;
    public GameObject ps;
    public bool isUsingController = false;

    private void Start() {
        xbox.SetActive(Hinput.anyGamepad.isConnected);
        ps.SetActive(Hinput.anyGamepad.isConnected);
        isUsingController = Globals.ControllerManager.IsUsingController();
        UpdateVisuals();
    }

    private void OnEnable() {
        UpdateVisuals();
    }

    void Update() {
        if (isUsingController != Globals.ControllerManager.IsUsingController()) UpdateVisuals();
    }

    private void UpdateVisuals() {
        if (Globals.IsInitialized()) {
            xbox.SetActive(Globals.ControllerManager.IsUsingController());
            ps.SetActive(Globals.ControllerManager.IsUsingController());
            isUsingController = Globals.ControllerManager.IsUsingController();
        }
    }
}
