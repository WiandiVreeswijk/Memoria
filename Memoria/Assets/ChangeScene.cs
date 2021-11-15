using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {
    public string sceneName;
    public void ActivateEnter() {
        Debug.Log("Activate scene!");
        Globals.SceneManager.SetScene(sceneName);
    }
}
