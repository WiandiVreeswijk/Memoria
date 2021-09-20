using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Localisator : MonoBehaviour {
    private TMP_Dropdown dropdown;

    public  TextMeshProUGUI playText, optionsText, quitText, optionsMenuText, languageOptionText, backOptionsMenuText, pauseMenuTitleText, resumeText, quitPauseMenuText, backToWijkText ;


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
        playText.text = "SPEEL";
        optionsText.text = "OPTIES";
        quitText.text = "VERLATEN";
        optionsMenuText.text = "OPTIES";
        languageOptionText.text = "TAALKEUZE";
        backOptionsMenuText.text = "TERUG";
        pauseMenuTitleText.text = "PAUZE";
        resumeText.text = "HERVATTEN";
        quitPauseMenuText.text = "VERLATEN";
        backToWijkText.text = "TERUG NAAR DE WIJK";
    }

    public void SetEnglish() {
        DialogueManager.SetLanguage("");
        playText.text = "PLAY";
        optionsText.text = "OPTIONS";
        quitText.text = "QUIT";
        optionsMenuText.text = "OPTIONS";
        languageOptionText.text = "LANGUAGE";
        backOptionsMenuText.text = "BACK";
        pauseMenuTitleText.text = "PAUSE";
        resumeText.text = "RESUME";
        quitPauseMenuText.text = "QUIT";
        backToWijkText.text = "BACK TO NEIGHBOURHOOD";
    }
}
