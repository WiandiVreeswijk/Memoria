using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ControllerButtonTrigger : MonoBehaviour {
    private enum ControllerButton {
        DOWN,
        UP,
        LEFT,
        RIGHT
    }

    private static Dictionary<ControllerButton, string> controllerButtons = new Dictionary<ControllerButton, string>()
    {
        {ControllerButton.DOWN, "joystick button 1"},
        {ControllerButton.UP, "joystick button 3"},
        {ControllerButton.LEFT, "joystick button 0"},
        {ControllerButton.RIGHT, "joystick button 2"}
    };

    [SerializeField] private ControllerButton button;

    Button buttonComponent;

    // Start is called before the first frame update
    void Start() {
        buttonComponent = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update() {
        if (buttonComponent != null) {
            if (Input.GetKeyDown(controllerButtons[button])) {
                buttonComponent.onClick.Invoke();
            }
        }
    }
}
