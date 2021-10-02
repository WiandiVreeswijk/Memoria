using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour {
    [SerializeField] private Volume volume;

    [SerializeField] private Canvas overlayCanvas;
    public Canvas OverlayCanvas => overlayCanvas;

    [SerializeField] private Canvas screenspaceCanvas;
    public Canvas ScreenspaceCanvas => screenspaceCanvas;


    private ChasingLevelUI chasingLevelUI;
    public ChasingLevelUI ChasingLevel => chasingLevelUI;

    private NotificationManager notificationManager;
    public NotificationManager NotificationManager => notificationManager;

    private MainMenu mainMenu;
    public MainMenu MainMenu => mainMenu;

    private PauseMenu pauseMenu;
    public PauseMenu PauseMenu => pauseMenu;

    private OptionsMenu optionsMenu;
    public OptionsMenu OptionsMenu => optionsMenu;

    public void OnGlobalsInitialize() {
        Utils.FindUniqueObjectInChildren(gameObject, out mainMenu);
        Utils.FindUniqueObjectInChildren(gameObject, out pauseMenu);
        Utils.FindUniqueObjectInChildren(gameObject, out optionsMenu);

        Utils.FindUniqueObject(out chasingLevelUI);
        Utils.FindUniqueObject(out notificationManager);
        screenspaceCanvas.worldCamera = Globals.Camera;
    }

    public void OnGlobalsInitializeType(Globals.GlobalsType type) {
        pauseMenu.SetBackToWijkButtonEnabled(type != Globals.GlobalsType.NEIGHBORHOOD);
        ChasingLevel.SetEnabled(type == Globals.GlobalsType.OBLIVION);

        switch (type) {
            case Globals.GlobalsType.DEBUG: OnGlobalsInitializeDebug(); break;
            case Globals.GlobalsType.NEIGHBORHOOD: OnGlobalsInitializeNeighborhood(); break;
            case Globals.GlobalsType.OBLIVION: OnGlobalsInitializeOblivion(); break;
        }
    }

    private void OnGlobalsInitializeDebug() {

    }

    private void OnGlobalsInitializeNeighborhood() {

    }

    private void OnGlobalsInitializeOblivion() {

    }

    public void SetDepthOfField(bool toggle) {
        volume.enabled = toggle;
    }
}
