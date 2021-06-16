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

    public GameObject persistenceManagerPrefab;
    public GameObject menuControllerPrefab;

    //Oblivion
    private OblivionManager oblivionManager;
    private CheckpointManager checkpointManager;

    private void Awake() {
        DontDestroyOnLoad(this);
        _Instance = this;
    }

    public static void Initialize(GlobalsType type) {
        if (_Instance == null) Debug.LogError("No globals found in scene.");
        _Instance.GlobalInitialize();
        switch (type) {
            case GlobalsType.NEIGHTBORHOOD: _Instance.InitializeNeighborhood(); break;
            case GlobalsType.OBLIVION: _Instance.InitializeOblivion(); break;
        }
    }

    private void GlobalInitialize() {
        player = Utils.FindUniqueObject<Player>();
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
        checkpointManager = Utils.FindUniqueObject<CheckpointManager>();
    }

    #region GlobalGlobals

    public static MenuController MenuController { get { return _Instance.menuController; } }
    public static PersistenceManager PersistenceManager { get { return _Instance.persistenceManager; } }
    public static Player Player { get { return _Instance.player; } }

    #endregion

    #region NeighbourhoodGlobals

    #endregion

    #region OblivionGlobals

    public static OblivionManager OblivionManager { get { return _Instance.oblivionManager; } }
    public static CheckpointManager CheckpointManager { get { return _Instance.checkpointManager; } }

    #endregion
}
