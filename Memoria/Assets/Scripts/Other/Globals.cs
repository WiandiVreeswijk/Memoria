using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    [Serializable]
    public enum GlobalsType {
        DEBUG,
        NEIGHBORHOOD,
        OBLIVION
    }

    private static Globals _Instance;
    private GlobalsType currentGlobalsType;

    //Global
    private Player player;
    private MenuController menuController;
    private Persistence persistenceManager;
    private SceneManager sceneManager;
    private Screenshake screenshake;
    private ScoreManager scoreManager;
    private Debugger debugger;
    private AnalyticsManager analyticsManager;
    private new Camera camera;
    private bool isInitialized = false;

    //UI
    private UIManager uiManager;

    //Prefabs
    public GameObject persistenceManagerPrefab;

    //Wijk
    private InteractableManager interactableManager;
    private MemoryWatchManager memoryWatchManager;

    //Oblivion
    private OblivionManager oblivionManager;
    private OblivionVFXManager oblivionVFXManager;
    private CheckpointManager checkpointManager;
    private SoundManagerChase soundManagerChase;


    private void Awake() {
        if (_Instance != null) {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else {
            _Instance = this;
            DontDestroyOnLoad(this);
            Utils.FindUniqueObject(out sceneManager);
            //SceneManager.LoadUI();
        }
    }

    public static bool IsInitialized() {
        return _Instance != null && _Instance.isInitialized;
    }

    public static void Initialize(GlobalsType type) {
        if (_Instance == null) Debug.LogError("No globals found in scene.");
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
        Debugger.InitializeMenu(type);
        _Instance.isInitialized = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            Initialize(GlobalsType.OBLIVION);
        }
    }

    private void GlobalInitialize() {
        Utils.FindUniqueObject(out MainCamera mainCamera);
        camera = mainCamera.GetComponent<Camera>();
        Utils.FindUniqueObject(out player);
        Utils.FindUniqueObject(out screenshake);
        Utils.FindUniqueObject(out scoreManager);
        Utils.FindUniqueObject(out analyticsManager);
        Utils.FindUniqueObject(out debugger);
        Utils.FindUniqueObject(out uiManager);
        Utils.FindUniqueObject(out menuController);
        Utils.FindUniqueObject(out memoryWatchManager);
        Utils.FindOrInstantiateUniqueObject(out persistenceManager, () => Instantiate(persistenceManagerPrefab, transform).GetComponent<Persistence>());

        uiManager.OnGlobalsInitialize();
    }

    private void InitializeDebug() {
    }

    private void InitializeNeighborhood() {
        Utils.FindUniqueObject(out interactableManager);
        menuController.SetMenu("Main", 0.0f);
    }

    private void InitializeOblivion() {
        oblivionManager = Utils.FindUniqueObject<OblivionManager>();
        oblivionVFXManager = Utils.FindUniqueObject<OblivionVFXManager>();
        checkpointManager = Utils.FindUniqueObject<CheckpointManager>();
        soundManagerChase = Utils.FindUniqueObject<SoundManagerChase>();
    }

    #region GlobalGlobals

    public static MenuController MenuController => _Instance.menuController;
    public static Persistence Persistence => _Instance.persistenceManager;
    public static Screenshake Screenshake => _Instance.screenshake;
    public static SceneManager SceneManager => _Instance.sceneManager;
    public static UIManager UIManager => _Instance.uiManager;
    public static ScoreManager ScoreManager => _Instance.scoreManager;
    public static AnalyticsManager AnalyticsManager => _Instance.analyticsManager;
    public static Player Player => _Instance.player;
    public static Debugger Debugger => _Instance.debugger;
    public static Camera Camera => _Instance.camera;

    #endregion

    #region NeighbourhoodGlobals
    public static InteractableManager InteractableManager => _Instance.interactableManager;
    public static MemoryWatchManager MemoryWatchManager => _Instance.memoryWatchManager;
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
