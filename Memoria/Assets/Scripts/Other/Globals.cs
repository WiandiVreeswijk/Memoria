using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    [Serializable]
    public enum GlobalsType {
        NEIGHTBORHOOD,
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
    private Debugger debugger;
    private bool isInitialized = false;

    //UI
    private UIManager uiManager;

    //Prefabs
    public GameObject persistenceManagerPrefab;

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
            SceneManager.LoadUI();
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
            case GlobalsType.NEIGHTBORHOOD:
                _Instance.InitializeNeighborhood();
                break;
            case GlobalsType.OBLIVION:
                _Instance.InitializeOblivion();
                break;
        }
        _Instance.isInitialized = true;
    }

    private void GlobalInitialize() {
        Utils.FindUniqueObject(out player);
        Utils.FindUniqueObject(out screenshake);
        Utils.FindOrInstantiateUniqueObject(out persistenceManager, () => Instantiate(persistenceManagerPrefab, transform).GetComponent<Persistence>());
    }

    public static void InitializeUI() {
        _Instance._InitializeUI();
    }

    public void _InitializeUI() {
        Utils.FindUniqueObject(out debugger);
        Utils.FindUniqueObject(out uiManager);
        Utils.FindUniqueObject(out menuController);
        if (currentGlobalsType == GlobalsType.NEIGHTBORHOOD) menuController.SetMenu("Main", 0.0f);
    }

    private void InitializeNeighborhood() {

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
    public static Player Player => _Instance.player;
    public static Debugger Debugger => _Instance.debugger;

    #endregion

    #region NeighbourhoodGlobals

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
