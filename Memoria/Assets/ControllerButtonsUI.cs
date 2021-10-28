using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonsUI : MonoBehaviour {
    public GameObject xbox;
    public GameObject ps;

    private void Start() {
        xbox.SetActive(Hinput.anyGamepad.isConnected);
        xbox.SetActive(Hinput.anyGamepad.isConnected);
    }
    void Update()
    {
        print(Globals.ControllerManager.JustChanged());
        if (Globals.ControllerManager.JustChanged()) {
            xbox.SetActive(Globals.ControllerManager.IsUsingController());
            ps.SetActive(Globals.ControllerManager.IsUsingController());
        }
    }
}
