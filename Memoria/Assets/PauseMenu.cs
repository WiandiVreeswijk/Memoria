using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject backToWijkButton;

    public void SetBackToWijkButtonEnabled(bool toggle) {
        backToWijkButton.SetActive(toggle);
    }
}
