using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;

public class Globals : MonoBehaviour {
    [Serializable]
    public enum GlobalsType {
        DEBUG,
        NEIGHBORHOOD,
        OBLIVION
    }

    private static Globals _Instance;
    private GlobalsType previousGlobalsType;
    private GlobalsType currentGlobalsType;

    //Global
    private Player player;
    private MenuController menuController;
    private Persistence persistenceManager;
    private ProgressionManagerOld progressionManager;
    private CinemachineManager cinemachineManager;
    private TimescaleManager timescaleManager;
    private SceneManager sceneManager;
    private ScreenshakeManager screenshakeManager;
    private ScoreManager scoreManager;
    private Debugger debugger;
    private CursorManager cursorManager;
    private AnalyticsManager analyticsManager;
    private GlobalTextTable localization;
    private new Camera camera;
    private bool isInitialized = false;
    private ControllerManager controllerManager;

    //UI
    private UIManager uiManager;
    private IconManager iconManager;

    //Prefabs
    public GameObject persistenceManagerPrefab;

    //Wijk
    private InteractableManager interactableManager;
    private MemoryWatchManager memoryWatchManager;
    private SoundManagerWijk soundManagerWijk;
    private MusicManagerWijk musicManagerWijk;
    private TrophyManager trophyManager;

    //Oblivion
    private OblivionManager oblivionManager;
    private OblivionVFXManager oblivionVFXManager;
    private CheckpointManager checkpointManager;
    private SoundManagerChase soundManagerChase;
    private AmbientControl ambientControl;


    private void Awake() {
        if (_Instance != null) {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else {
            _Instance = this;
            DontDestroyOnLoad(this);
            Utils.FindUniqueObject(out sceneManager);
        }
    }

    public static bool IsInitialized() {
        return _Instance != null && _Instance.isInitialized;
    }

    public static GlobalsType GetCurrentGlobalsType() {
        return _Instance.currentGlobalsType;
    }

    public static void Initialize(GlobalsType type) {
        if (_Instance == null) Debug.LogError("No globals found in scene.");
        _Instance.previousGlobalsType = _Instance.currentGlobalsType;
        _Instance.currentGlobalsType = type;
        _Instance.GlobalInitialize();
        switch (type) {
            case GlobalsType.DEBUG:
                _Instance.InitializeDebug();
                break;
            case GlobalsType.NEIGHBORHOOD:
                _Instance.InitializeNeighborhood();
                break;
            case GlobalsType.OBLIVION:
                _Instance.InitializeOblivion();
                break;
        }
        _Instance.PostGlobalsInitialize();
        Debugger.InitializeMenu(type);

        _Instance.isInitialized = true;
    }

    private void GlobalInitialize() {
        Utils.FindUniqueObject(out MainCamera mainCamera);
        Utils.FindUniqueObject(out iconManager);
        Utils.FindUniqueObject(out scoreManager);
        Utils.FindUniqueObject(out cursorManager);
        Utils.FindUniqueObject(out progressionManager);
        Utils.FindUniqueObject(out screenshakeManager);
        Utils.FindUniqueObject(out analyticsManager, false);
        Utils.FindUniqueObject(out debugger);
        Utils.FindUniqueObject(out uiManager);
        Utils.FindUniqueObject(out localization);
        Utils.FindUniqueObject(out menuController);
        Utils.FindUniqueObject(out timescaleManager);
        Utils.FindUniqueObject(out cinemachineManager);
        Utils.FindUniqueObject(out memoryWatchManager, false);
        Utils.FindOrInstantiateUniqueObject(out persistenceManager, () => Instantiate(persistenceManagerPrefab, transform).GetComponent<Persistence>());
        Utils.FindUniqueObject(out player);
        Utils.FindUniqueObject(out controllerManager);

        camera = mainCamera.GetComponent<Camera>();
    }

    private void InitializeDebug() {
    }

    //#Todo: this is dirty
    private bool firstTime = true;
    private void InitializeNeighborhood() {
        Utils.FindUniqueObject(out interactableManager);
        Utils.FindUniqueObject(out soundManagerWijk);
        Utils.FindUniqueObject(out musicManagerWijk);
        Utils.FindUniqueObject(out trophyManager);
        Utils.FindUniqueObject(out ambientControl);
        if (firstTime) {
            menuController.SetMenu("Main", 0.0f);
            firstTime = false;
        }
    }

    private void InitializeOblivion() {
        oblivionManager = Utils.FindUniqueObject<OblivionManager>();
        oblivionVFXManager = Utils.FindUniqueObject<OblivionVFXManager>();
        checkpointManager = Utils.FindUniqueObject<CheckpointManager>();
        soundManagerChase = Utils.FindUniqueObject<SoundManagerChase>();
        UIManager.SetDepthOfField(false);
    }

    private void PostGlobalsInitialize() {
        player?.OnGlobalsInitialize();
        uiManager?.OnGlobalsInitializeType(currentGlobalsType);
        trophyManager?.OnGlobalsInitializeType(previousGlobalsType, currentGlobalsType);
        player?.OnGlobalsInitializeType(previousGlobalsType, currentGlobalsType);
        progressionManager?.OnGlobalsInitializeType(previousGlobalsType, currentGlobalsType);
        screenshakeManager?.OnGlobalsInitializeType(previousGlobalsType, currentGlobalsType);
    }

    #region GlobalGlobals

    public static MenuController MenuController => _Instance.menuController;
    public static Persistence Persistence => _Instance.persistenceManager;
    public static CinemachineManager CinemachineManager => _Instance.cinemachineManager;
    public static ProgressionManagerOld ProgressionManager => _Instance.progressionManager;
    public static TimescaleManager TimescaleManager => _Instance.timescaleManager;
    public static ScreenshakeManager Screenshake => _Instance.screenshakeManager;
    public static SceneManager SceneManager => _Instance.sceneManager;
    public static GlobalTextTable Localization => _Instance.localization;
    public static UIManager UIManager => _Instance.uiManager;
    public static IconManager IconManager => _Instance.iconManager;
    public static ScoreManager ScoreManager => _Instance.scoreManager;
    public static AnalyticsManager AnalyticsManager => _Instance.analyticsManager;
    public static Player Player => _Instance.player;
    public static Debugger Debugger => _Instance.debugger;
    public static CursorManager CursorManager => _Instance.cursorManager;
    public static Camera Camera => _Instance.camera;
    public static ControllerManager ControllerManager => _Instance.controllerManager;

    #endregion

    #region WijkGlobals
    public static InteractableManager InteractableManager => _Instance.interactableManager;
    public static MemoryWatchManager MemoryWatchManager => _Instance.memoryWatchManager;
    public static SoundManagerWijk SoundManagerWijk => _Instance.soundManagerWijk;
    public static MusicManagerWijk MusicManagerWijk => _Instance.musicManagerWijk;
    public static TrophyManager TrophyManager => _Instance.trophyManager;

    public static AmbientControl AmbientControl => _Instance.ambientControl;
    #endregion

    #region OblivionGlobals

    public static OblivionManager OblivionManager => _Instance.oblivionManager;
    public static OblivionVFXManager OblivionVFXManager => _Instance.oblivionVFXManager;
    public static CheckpointManager CheckpointManager => _Instance.checkpointManager;
    public static SoundManagerChase SoundManagerChase => _Instance.soundManagerChase;

    #endregion

    public static Globals GetInstance() {
        return _Instance;
    }
}
