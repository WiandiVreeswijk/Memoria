using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneActivatable : MonoBehaviour, IEnterActivatable {
    public string sceneName;

    public void ActivateEnter() {
        Globals.SceneManager.SetScene(sceneName);
    }
}
