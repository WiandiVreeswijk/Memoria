using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    private static Globals _Instance;
    private PersistenceManager persistenceManager;

    void Start() {
        _Instance = this;
        DontDestroyOnLoad(this);
        persistenceManager = Utils.FindUniqueScript<PersistenceManager>();
    }

    public static PersistenceManager GetPersistenceManager() {
        return _Instance.persistenceManager;
    }
}
