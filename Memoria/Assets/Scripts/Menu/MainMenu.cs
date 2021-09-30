using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using DG.Tweening;

public class MainMenu : MonoBehaviour {
    public GameObject controlPanel;

    public CinemachineVirtualCamera mainMenuCamera;

    private void Start() {
        Globals.MusicManagerWijk.FadePianoVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeViolinVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeHarpVolume(1.0f, 2.0f);
        Globals.SoundManagerWijk.FadeAmbientVolume(0.0f, 0.0f);
        Globals.AmbientControl.SetVolume(0);
    }

    public void PlayGame() {
        Globals.MenuController.BlackScreenFadeIn(0.0f, false);
        Utils.DelayedAction(6, () => Globals.MenuController.BlackScreenFadeOut(2.5f));
        FindObjectOfType<WijkOpeningCutscene>().Trigger();
        Globals.MenuController.CloseMenu(0.5f);
        Globals.SoundManagerWijk.FadeEngineStatus(1.0f);
        Globals.SoundManagerWijk.FadeEngineVolume(0.4f);
        Globals.MusicManagerWijk.FadeFluteVolume(0.9f, 10.0f);
        
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
