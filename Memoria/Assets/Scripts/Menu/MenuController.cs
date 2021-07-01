using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    //private Animator animator;
    // Start is called before the first frame update

    [Serializable]
    public struct WAEMUIElement {
        public string name;
        public CanvasGroup panel;
    }

    public class WAEMDictUIElement {
        public CanvasGroup panel;
        public Tween tween;

        public WAEMDictUIElement(CanvasGroup panel) {
            this.panel = panel;
            this.tween = null;
        }
    }

    public GameObject collectedThingies;
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationsParent;

    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private WAEMUIElement[] uiElements;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private CanvasGroup blackScreen;

    private Tween mainFade;
    private WAEMDictUIElement activePanel;
    private Dictionary<string, WAEMDictUIElement> uiElementsDict = new Dictionary<string, WAEMDictUIElement>();
    private int notificationIndex = 0;


    private bool initialized = false;
    private string afterInitializationPanel = "";
    void Start() {
        foreach (var element in uiElements) {
            if (element.panel != null) {
                uiElementsDict.Add(element.name, new WAEMDictUIElement(element.panel));
                element.panel.blocksRaycasts = false;
                element.panel.alpha = 0.0f;
            } else Debug.LogError($"A UIElement named {element.name} is null");
        }

        CloseMenu(true);
        initialized = true;
        if (afterInitializationPanel.Length != 0) SetMenu(afterInitializationPanel, 0.0f);
    }

    private Tween blackScreenTween;
    public Tween BlackScreenFadeIn(float duration) {
        blackScreenTween?.Kill();
        return DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 1.0f, duration).OnComplete(() => {
            blackScreen.blocksRaycasts = false;
            blackScreen.interactable = false;
        });
    }

    public Tween BlackScreenFadeOut(float duration) {
        blackScreenTween?.Kill();
        blackScreen.blocksRaycasts = false;
        blackScreen.interactable = false;
        return DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 0.0f, duration);
    }

    public void ReloadScene() {
        Application.Quit();
        //DOTween.Clear();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseMenu(bool instant = false) {
        if (activePanel != null) {
            FadePanelOut(activePanel, instant ? 0.0f : fadeTime);
            activePanel = null;
        }
        FadeMainPanelOut(instant ? 0.0f : fadeTime);
    }

    public void SetMenu(string name) {
        if (name == "Notification") collectedThingies.SetActive(true);
        SetMenu(name, fadeTime);
    }

    public void SetMenu(string name, float time) {
        if (!initialized) {
            afterInitializationPanel = name;
            return;
        }
        if (uiElementsDict.TryGetValue(name, out WAEMDictUIElement group)) {
            if (activePanel == group) return;
            if (activePanel == null) FadeMainPanelIn(time);
            if (activePanel != null) FadePanelOut(activePanel, time / 2.0f);
            FadePanelIn(group, time);
            activePanel = group;
        } else Debug.LogError($"UIElement {name} is unknown");
    }

    public void NotifyPlayer(string message, float delay = 0.0f) {
        GameObject obj = Instantiate(notificationPrefab, notificationsParent.transform);
        var rectTransform = obj.GetComponent<RectTransform>();
        obj.transform.position += new Vector3(0, (rectTransform.rect.height + 10f) * notificationIndex, 0);
        var group = obj.GetComponent<CanvasGroup>();
        group.alpha = 0.0f;

        var sequence = DOTween.Sequence()
            .AppendInterval(delay)
            .AppendCallback(() => { notificationIndex++; })
            .Append(DOTween.To(() => group.alpha, x => group.alpha = x, 1.0f, 1.5f))
            .AppendInterval(5.0f)
            .Append(DOTween.To(() => group.alpha, x => group.alpha = x, 0.0f, 1.5f))
            .AppendCallback(() => {
                Destroy(obj);
                notificationIndex--;
            });
    }

    private void FadePanelIn(WAEMDictUIElement group, float time) {
        group.tween?.Kill();
        group.panel.blocksRaycasts = true;
        group.panel.gameObject.SetActive(true);
        group.tween = DOTween.To(() => group.panel.alpha, x => group.panel.alpha = x, 1.0f, time).SetEase(Ease.OutQuint);
    }

    private void FadePanelOut(WAEMDictUIElement group, float time) {
        group.tween?.Kill();
        group.panel.blocksRaycasts = false;
        group.tween = DOTween.To(() => group.panel.alpha, x => group.panel.alpha = x, 0.0f, time).OnComplete(() => {
            group.panel.gameObject.SetActive(false);
        }).SetEase(Ease.OutQuint);
    }

    private void FadeMainPanelIn(float time) {
        mainFade?.Kill(true);
        mainPanel.gameObject.SetActive(true);
        mainFade = DOTween.To(() => mainPanel.alpha, x => mainPanel.alpha = x, 1.0f, time).SetEase(Ease.InCirc);
    }

    private void FadeMainPanelOut(float time) {
        mainFade?.Kill(true);
        mainFade = DOTween.To(() => mainPanel.alpha, x => mainPanel.alpha = x, 0.0f, time).OnComplete(() => {
            mainPanel.gameObject.SetActive(false);
        }).SetEase(Ease.InCirc);
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) NotifyPlayer("Hallo!");
        //if (Input.GetKeyDown(KeyCode.Alpha2)) NotifyPlayer("Notificatie!");
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CloseMenu();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetMenu("Main");
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetMenu("Options");
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
