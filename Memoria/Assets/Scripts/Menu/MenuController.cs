using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    [Serializable]
    public struct WAEMUIElement {
        public string name;
        public CanvasGroup panel;
        public bool background;
    }

    public class WAEMDictUIElement {
        public CanvasGroup panel;
        public Tween tween;
        public string name;
        public bool background;

        public WAEMDictUIElement(CanvasGroup panel, string name, bool background) {
            this.panel = panel;
            this.tween = null;
            this.name = name;
            this.background = background;
        }
    }

    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private WAEMUIElement[] uiElements;
    [SerializeField] private float fadeTime = 0.125f;
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private Image image;
    [SerializeField] private GameObject redBackground;
    public TextMeshProUGUI blackScreenText;
    private bool blackScreenActive = false;
    private CinemachineVirtualCamera menuCamera;

    private Tween mainFade;
    private WAEMDictUIElement activePanel;
    private Dictionary<string, WAEMDictUIElement> uiElementsDict = new Dictionary<string, WAEMDictUIElement>();

    private bool initialized = false;
    private string afterInitializationPanel = "";

    void Start() {
        foreach (var element in uiElements) {
            if (element.panel != null) {
                uiElementsDict.Add(element.name, new WAEMDictUIElement(element.panel, element.name, element.background));
                element.panel.blocksRaycasts = false;
                element.panel.alpha = 0.0f;
            } else Debug.LogError($"A UIElement named {element.name} is null");
        }

        menuCamera = FindObjectOfType<MenuCamera>()?.GetComponent<CinemachineVirtualCamera>();
        blackScreen.gameObject.SetActive(true);
        BlackScreenFadeOut(0.0f);
        CloseMenu(0.0f);
        initialized = true;
        if (afterInitializationPanel.Length != 0) SetMenu(afterInitializationPanel, 0.0f);
    }

    private Tween blackScreenTween;
    public Tween BlackScreenFadeIn(float duration, bool loadingIcon) {
        image.gameObject.SetActive(loadingIcon);
        blackScreenTween?.Kill();
        blackScreenActive = true;
        return DOTween.Sequence().Append(DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 1.0f, duration).OnComplete(() => {
            blackScreen.blocksRaycasts = false;
            blackScreen.interactable = false;
        })).SetUpdate(true);
    }

    public Tween BlackScreenFadeOut(float duration) {
        blackScreenTween?.Kill();
        blackScreen.blocksRaycasts = false;
        blackScreen.interactable = false;
        blackScreenActive = false;
        return DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 0.0f, duration).SetUpdate(true)
            .OnComplete(
                () => {
                });
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void CloseMenu() {
        CloseMenu(fadeTime);
    }

    public void CloseMenu(float fadeTime) {
        if (activePanel != null) {
            FadePanelOut(activePanel, fadeTime);
            activePanel = null;
        }

        if (fadeTime == 0.0f) {
            Globals.UIManager.SetDepthOfField(false);
            Globals.TimescaleManager.UnPauseGame();
            if (menuCamera) menuCamera.Priority = 0;
        } else {
            FadeMainPanelOut(fadeTime).OnComplete(() => {
                Globals.UIManager.SetDepthOfField(false);
                Globals.TimescaleManager.UnPauseGame();
                if (menuCamera) menuCamera.Priority = 0;
            });
        }
        Globals.CursorManager.LockMouse();
    }

    public void SetMenu(string name) {
        SetMenu(name, fadeTime);
    }

    public string GetActiveMenu() {
        return activePanel == null ? "" : activePanel.name;
    }

    public void SetMenu(string name, float time) {
        if (!initialized) {
            afterInitializationPanel = name;
            return;
        }
        if (uiElementsDict.TryGetValue(name, out WAEMDictUIElement group)) {
            if (activePanel == group) return;
            redBackground.SetActive(group.background);
            if (group.background) {
                Globals.TimescaleManager.PauseGame();
            }
            if(menuCamera)menuCamera.Priority = group.background ? 0 : 12;
            if (activePanel == null) FadeMainPanelIn(time);
            if (activePanel != null) FadePanelOut(activePanel, time / 2.0f);
            FadePanelIn(group, time);
            Globals.CursorManager.UnlockMouse();
            activePanel = group;
        } else Debug.LogError($"UIElement {name} is unknown");
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !Globals.Debugger.IsOpen() && DialogueManager.CurrentConversant == null) {
            TogglePause();
        }
    }

    private void FadePanelIn(WAEMDictUIElement group, float time) {
        group.tween?.Kill();
        group.panel.blocksRaycasts = true;
        group.panel.gameObject.SetActive(true);
        if (time == 0.0f) {
            group.panel.alpha = 1.0f;
        } else {
            group.tween = DOTween.To(() => group.panel.alpha, x => group.panel.alpha = x, 1.0f, time).SetEase(Ease.OutQuint).SetUpdate(true);
        }
    }

    private void FadePanelOut(WAEMDictUIElement group, float time) {
        group.tween?.Kill();
        group.panel.blocksRaycasts = false;
        if (time == 0.0f) {
            group.panel.alpha = 0.0f;
            group.panel.gameObject.SetActive(false);
        } else {
            group.tween = DOTween.To(() => group.panel.alpha, x => group.panel.alpha = x, 0.0f, time)
                .OnComplete(() => { group.panel.gameObject.SetActive(false); }).SetUpdate(true);
        }
    }

    private void FadeMainPanelIn(float time) {
        mainFade?.Kill();
        mainPanel.blocksRaycasts = true;
        mainPanel.gameObject.SetActive(true);
        Globals.UIManager.SetDepthOfField(true);
        if (time == 0.0f) {
            mainPanel.alpha = 1.0f;
        } else {
            mainFade = DOTween.To(() => mainPanel.alpha, x => mainPanel.alpha = x, 1.0f, time).SetUpdate(true);
        }
    }

    private Tween FadeMainPanelOut(float time) {
        mainFade?.Kill();
        mainPanel.blocksRaycasts = false;
        if (time == 0.0f) {
            mainPanel.alpha = 0.0f;
            mainPanel.gameObject.SetActive(false);
        } else {
            mainFade = DOTween.To(() => mainPanel.alpha, x => mainPanel.alpha = x, 0.0f, time)
                .OnComplete(() => { mainPanel.gameObject.SetActive(false); }).SetEase(Ease.InCirc).SetUpdate(true);
        }

        return mainFade;
    }

    public bool IsOpen() {
        return GetActiveMenu().Length != 0;
    }

    private void TogglePause() {
        string active = GetActiveMenu();
        if (active == "" && !blackScreenActive) {
            SetMenu("Pause");
        } else if (active == "Pause") {
            CloseMenu();
        }
    }

    public void BackToWijk() {
        Globals.SceneManager.SetScene("Wijk");
    }

    // Update is called once per frame
    //void Update() {
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        CloseMenu();
    //    if (Input.GetKeyDown(KeyCode.S))
    //        OpenMenu("MainMenu");
    //    if (Input.GetKeyDown(KeyCode.E))
    //        OpenMenu("OptionsMenu");
    //}
    //
    //public void CloseMenu() {
    //    ResetTrigger("OpenMenu");
    //    Trigger("Exit");
    //    Trigger("CloseMenu");
    //}
    //
    //public void OpenMenu(string menu) {
    //    ResetTrigger("CloseMenu");
    //    Trigger(menu);
    //}
    //
    //private void Trigger(string trigger) {
    //    animator.SetTrigger(trigger);
    //}
    //
    //private void ResetTrigger(string trigger) {
    //    animator.ResetTrigger(trigger);
    //}
}
