using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Localisator : MonoBehaviour {
    private TMPro.TMP_Dropdown dropdown;


    public void Start() {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChange);
    }

    public void OnValueChange(int value) {
        switch (dropdown.value) {
            case 0: SetEnglish(); break;
            case 1: SetDutch(); break;
        }
    }

    public void SetDutch() {
        DialogueManager.SetLanguage("nl");
    }

    public void SetEnglish() {
        DialogueManager.SetLanguage("");
    }
}
