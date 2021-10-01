using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using DG.Tweening;

public class MainMenu : MonoBehaviour {
    private void Start() {
        Globals.MusicManagerWijk.FadePianoVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeViolinVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeHarpVolume(1.0f, 2.0f);
        Globals.SoundManagerWijk.FadeAmbientVolume(0.0f, 0.0f);
        Globals.AmbientControl.SetVolume(0);
    }

    public void PlayGame() {
        Globals.CinemachineManager.SetInputEnabled(false);
        Globals.MenuController.BlackScreenFadeIn(1.0f, false).OnComplete(() => {
            Globals.UIManager.EnableBackground();
            FindObjectOfType<MenuCamera>().cam.Priority = 0;
            FindObjectOfType<WijkOpeningCutscene>().Trigger();
        });
        Utils.DelayedAction(6, () => {
            Globals.CinemachineManager.SetInputEnabled(true);
            Globals.MenuController.BlackScreenFadeOut(2.5f);
        });
        Globals.MenuController.CloseMenu(1.0f);
        Globals.SoundManagerWijk.FadeEngineStatus(1.0f);
        Globals.SoundManagerWijk.FadeEngineVolume(0.4f);
        Globals.MusicManagerWijk.FadeFluteVolume(0.9f, 10.0f);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
