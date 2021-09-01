using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour {
    private bool debugMenuVisible = false;
    private bool isActive = false;
    private DebugPrint debugPrint;

    /*Debug menu*/
    public GameObject debuggerButtonPrefab;
    public GameObject debuggerLabelPrefab;
    public CanvasGroup debugMenuPanel;

    private void Awake() {
        isActive = Application.isEditor || Debug.isDebugBuild;
        debugMenuPanel.gameObject.SetActive(isActive);
        gameObject.SetActive(isActive);
    }
    private void Start() {
        debugPrint = GetComponent<DebugPrint>();
        SetVisible(false);
    }

    public void Print(string keyword, string text, float duration = -1) {
        if (!isActive) return;
        debugPrint.Print(keyword, text, duration);
    }

    public void ClearMenu() {
        foreach (Transform child in debugMenuPanel.transform) {
            Destroy(child.gameObject);
        }
    }

    public void AddLabel(string label) {
        GameObject labelObject = Instantiate(debuggerLabelPrefab, debugMenuPanel.transform);
        labelObject.GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    public void AddButton(string name, bool enabled, Action action) {
        DebuggerButton button = Instantiate(debuggerButtonPrefab, debugMenuPanel.transform).GetComponent<DebuggerButton>();
        button.Set(action, enabled, name);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            SetVisible(!debugMenuVisible);
        }
    }

    private void SetVisible(bool toggle) {
        if (toggle) {
            Time.timeScale = 0.0f;
            debugMenuPanel.alpha = 1.0f;
            debugMenuPanel.blocksRaycasts = true;
            debugMenuPanel.interactable = true;
            debugMenuVisible = true;
            debugPrint.SetTextVisible(false);
        } else {
            Time.timeScale = 1.0f;
            debugMenuPanel.alpha = 0.0f;
            debugMenuPanel.blocksRaycasts = false;
            debugMenuPanel.interactable = false;
            debugMenuVisible = false;
            debugPrint.SetTextVisible(true);
        }
    }

    public void InitializeMenu(Globals.GlobalsType type) {
        if (!isActive) return;
        ClearMenu();

        AddLabel("Debug menu");
        AddLabel("Scenes");

        foreach (var scene in Globals.SceneManager.GetAllLevelScenes()) {
            if (scene.name == "UI" || scene.name == "Persistent") continue;
            bool activeScene = scene != Globals.SceneManager.GetActiveScene();
            AddButton(scene.name, activeScene, () => {
                SetVisible(false);
                Globals.SceneManager.SetScene(scene.name);
            });
        }

        AddLabel("Camera");
        AddButton("Switch camera", true, ()=>{
            if(Globals.Player.CameraController.firstPersonCamera != null)
            {
                Globals.Player.CameraController.ToggleCamera();
            }
        });

        switch (type) {
            case Globals.GlobalsType.NEIGHBORHOOD: InitializeMenuNeighborhood(); break;
            case Globals.GlobalsType.OBLIVION: InitializeMenuOblivion(); break;
        }
    }

    private void InitializeMenuNeighborhood() {

    }

    private void InitializeMenuOblivion() {

    }
}