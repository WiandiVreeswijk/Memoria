using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneActivatable : MonoBehaviour, IActivatable {
    public string sceneName;

    public void Activate() {
        Globals.SceneManager.SetScene(sceneName);
    }
}
