using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    private static Globals _Instance;
    private Player player;
    private MenuController menuController;
    private PersistenceManager persistenceManager;

    void Start() {
        _Instance = this;
        //DontDestroyOnLoad(this);
        persistenceManager = Utils.FindUniqueObject<PersistenceManager>();
        menuController = Utils.FindUniqueObject<MenuController>();
        player = Utils.FindUniqueObject<Player>();
    }

    public static MenuController GetMenuController() {
        return _Instance.menuController;
    }

    public static PersistenceManager GetPersistenceManager() {
        return _Instance.persistenceManager;
    }

    public static Player GetPlayer() {
        return _Instance.player;
    }
}
