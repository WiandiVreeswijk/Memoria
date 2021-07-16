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
        FindObjectOfType<WijkOpeningCutscene>().Trigger();
        Globals.MenuController.CloseMenu();
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
