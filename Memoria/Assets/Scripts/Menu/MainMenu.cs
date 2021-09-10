using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using DG.Tweening;

public class MainMenu : MonoBehaviour {
    public GameObject controlPanel;

    public CinemachineVirtualCamera mainMenuCamera;

    public CursorLocker cursorLocker;

    private void Start() {
        //#Todo This is extremely dirty and will break very soon
        //cursorLocker.UnlockMouse();
    }

    public void PlayGame() {
        Globals.MenuController.BlackScreenFadeIn(2.0f).OnComplete(() => {

            Utils.DelayedAction(6,()=> Globals.MenuController.BlackScreenFadeOut(2.5f));
            FindObjectOfType<WijkOpeningCutscene>().Trigger();
            Globals.MenuController.CloseMenu();
            Globals.SoundManagerWijk.FadeEngineStatus(1.0f);
            Globals.SoundManagerWijk.FadeEngineVolume(0.4f);
            Globals.MusicManagerWijk.FadeMusicVolume(1.0f);
        });
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
