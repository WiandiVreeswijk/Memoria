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
        //#Todo This is extremely dirty and will break very soon
        //cursorLocker.UnlockMouse();

        Globals.MusicManagerWijk.FadePianoVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeViolinVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeHarpVolume(1.0f, 2.0f);
    }

    public void PlayGame() {
        Globals.MenuController.BlackScreenFadeIn(2.0f).OnComplete(() => {

            Utils.DelayedAction(6,()=> Globals.MenuController.BlackScreenFadeOut(2.5f));
            FindObjectOfType<WijkOpeningCutscene>().Trigger();
            Globals.MenuController.CloseMenu();
            Globals.SoundManagerWijk.FadeEngineStatus(1.0f);
            Globals.SoundManagerWijk.FadeEngineVolume(0.4f);
            Globals.MusicManagerWijk.FadeFluteVolume(1.0f, 5.0f);
        });
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
