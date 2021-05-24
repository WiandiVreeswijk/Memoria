using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    private static Globals _Instance;
    private MenuController menuController;
    private PersistenceManager persistenceManager;

    void Start() {
        _Instance = this;
        //DontDestroyOnLoad(this);
        persistenceManager = Utils.FindUniqueScript<PersistenceManager>();
        menuController = FindObjectThatShouldBeUnique<MenuController>();
    }

    public static MenuController GetMenuController() {
        return _Instance.menuController;
    }

    public static PersistenceManager GetPersistenceManager() {
        return _Instance.persistenceManager;
    }

    private static T FindObjectThatShouldBeUnique<T>() where T : Object {
        var objects = FindObjectsOfType<T>();
        if (objects.Length == 0) Debug.LogError($"No instances of unique type {typeof(T).Name} could be found. This will probably break a lot of stuff!");
        if (objects.Length == 1) return objects[0];
        if (objects.Length > 1) Debug.LogError($"Multiple instances of unique type {typeof(T).Name} could be found. This will probably break a lot of stuff!");
        return null;
    }
}
