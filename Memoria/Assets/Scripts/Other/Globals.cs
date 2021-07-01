using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    [Serializable]
    public enum GlobalsType {
        NEIGHTBORHOOD,
        OBLIVION,
    }

    private static Globals _Instance;

    //Global
    private Player player;
    private MenuController menuController;
    private PersistenceManager persistenceManager;
    private Debugger debugger;
    private bool isInitialized = false;

    public GameObject persistenceManagerPrefab;
    public GameObject menuControllerPrefab;
    public GameObject debuggerPrefab;

    //Oblivion
    private OblivionManager oblivionManager;
    private OblivionVFXManager oblivionVFXManager;
    private CheckpointManager checkpointManager;
    private SoundManagerChase soundManagerChase;

    private void Awake() {
        DontDestroyOnLoad(this);
        _Instance = this;
    }

    public static bool IsInitialized() {
        return _Instance != null && _Instance.isInitialized;
    }

    public static void Initialize(GlobalsType type) {
        if (_Instance == null) Debug.LogError("No globals found in scene.");
        _Instance.GlobalInitialize();
        switch (type) {
            case GlobalsType.NEIGHTBORHOOD: _Instance.InitializeNeighborhood(); break;
            case GlobalsType.OBLIVION: _Instance.InitializeOblivion(); break;
        }
        _Instance.isInitialized = true;
    }

    private void GlobalInitialize() {
        player = Utils.FindUniqueObject<Player>();

        Utils.FindOrInstantiateUniqueObject(out debugger, () => {
            return Instantiate(debuggerPrefab, transform).GetComponent<Debugger>();
        });

        Utils.FindOrInstantiateUniqueObject(out menuController, () => {
            return Instantiate(menuControllerPrefab, transform).GetComponent<MenuController>();
        });

        Utils.FindOrInstantiateUniqueObject(out persistenceManager, () => {
            return Instantiate(persistenceManagerPrefab, transform).GetComponent<PersistenceManager>();
        });
    }

    private void InitializeNeighborhood() {
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
    public static PersistenceManager Persistence => _Instance.persistenceManager;
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
}
