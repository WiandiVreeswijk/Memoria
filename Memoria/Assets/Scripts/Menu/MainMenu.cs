using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using DG.Tweening;
using Debug = UnityEngine.Debug;

public class MainMenu : MonoBehaviour {
    private void Start() {
        Globals.MusicManagerWijk.FadePianoVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeViolinVolume(1.0f, 2.0f);
        Globals.MusicManagerWijk.FadeHarpVolume(1.0f, 2.0f);
        Globals.SoundManagerWijk.FadeAmbientVolume(0.0f, 0.0f);
        Globals.AmbientControl.SetVolume(0);
    }

    private float textTime = 11.0f;
    private float fadeTime = 1.0f;
    private Tween updateTween;
    private Tween textTween;
    public void PlayGame() {
        Globals.MenuController.CloseMenu(1.0f);
        Globals.CinemachineManager.SetInputEnabled(false);
        Globals.MenuController.blackScreenText.text = "";
        if (FindObjectOfType<WijkOpeningCutscene>().ShouldSkip()) {
            Switch();
            return;
        }

        Globals.MenuController.BlackScreenFadeIn(1.0f, false).OnComplete(() => {
            updateTween = Utils.DelayedAction(60, () => { }).SetUpdate(true).OnUpdate(() => {
                if (Application.isEditor && Input.GetKeyDown(KeyCode.Space)) {
                    textTween.Kill();
                    Globals.MenuController.blackScreenText.DOFade(0, 0.1f).SetUpdate(true);
                    Globals.MenuController.blackScreenText.text = "";
                    Switch();
                }
            });
            textTween = Utils.DelayedAction(2.5f, () => {
                Globals.MenuController.blackScreenText.color = new Color(1, 1, 1, 0);
                Globals.MenuController.blackScreenText.text = Globals.Localization.Get("INTR_STORY_1");
                Globals.MenuController.blackScreenText.DOFade(1, fadeTime);
                textTween = Utils.DelayedAction(textTime, () => {
                    Globals.MenuController.blackScreenText.DOFade(0, fadeTime).OnComplete(() => {
                        Globals.MenuController.blackScreenText.text = Globals.Localization.Get("INTR_STORY_2");
                        Globals.MenuController.blackScreenText.DOFade(1, fadeTime);
                        textTween = Utils.DelayedAction(textTime, () => {
                            Globals.MenuController.blackScreenText.DOFade(0, fadeTime).OnComplete(() => {
                                Globals.MenuController.blackScreenText.text = Globals.Localization.Get("INTR_STORY_3");
                                Globals.MenuController.blackScreenText.DOFade(1, fadeTime);
                                textTween = Utils.DelayedAction(textTime, () => {
                                    Globals.MenuController.blackScreenText.DOFade(0, fadeTime).OnComplete(() => {
                                        Globals.MenuController.blackScreenText.text = Globals.Localization.Get("INTR_STORY_4");
                                        Globals.MenuController.blackScreenText.DOFade(1, fadeTime);
                                        textTween = Utils.DelayedAction(textTime / 2f, () => {
                                            Globals.MenuController.blackScreenText.DOFade(0, fadeTime).OnComplete(() => {
                                                Globals.MenuController.blackScreenText.text = Globals.Localization.Get("INTR_STORY_5");
                                                Globals.MenuController.blackScreenText.DOFade(1, fadeTime);
                                                Switch();
                                                textTween = Utils.DelayedAction(textTime * 0.7f, () => {
                                                    Globals.MenuController.blackScreenText.DOFade(0, fadeTime).SetUpdate(true);
                                                }).SetUpdate(true);
                                            }).SetUpdate(true);
                                        }).SetUpdate(true);
                                    }).SetUpdate(true);
                                }).SetUpdate(true);
                            }).SetUpdate(true);
                        }).SetUpdate(true);
                    }).SetUpdate(true);
                }).SetUpdate(true);
            }).SetUpdate(true);
        }).SetUpdate(true);
    }


    private void Switch() {
        updateTween.Kill();
        Globals.SoundManagerWijk.FadeEngineStatus(1.0f);
        Globals.SoundManagerWijk.FadeEngineVolume(0.4f);
        Globals.MusicManagerWijk.FadeFluteVolume(0.9f, 10.0f);
        FindObjectOfType<MenuCamera>().cam.Priority = 0;
        FindObjectOfType<WijkOpeningCutscene>().Trigger();
        Utils.DelayedAction(5, () => {
            Globals.CinemachineManager.SetInputEnabled(true);
            Globals.MenuController.BlackScreenFadeOut(2.5f);
        });
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
