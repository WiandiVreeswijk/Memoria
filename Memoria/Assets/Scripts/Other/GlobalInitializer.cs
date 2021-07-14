using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInitializer : MonoBehaviour {
    public Globals.GlobalsType type;

    private void Awake() {
        SceneManager.LoadSceneIfNotActive("UI");
        SceneManager.LoadSceneIfNotActive("Persistent");
    }

    void Start() {
        Globals.Initialize(type);
    }
}
