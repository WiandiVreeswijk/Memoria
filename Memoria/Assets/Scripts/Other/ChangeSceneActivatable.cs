using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneActivatable : MonoBehaviour, IEnterActivatable {
    public string sceneName;

    public void ActivateEnter() {
        Globals.SoundManagerChase.FadeVolume(0.0f);
        //Globals.SoundManagerChase.FadeAmbient(0.0f);
        Globals.Player.PlayerMovement25D.SetStunned(true, true, true);
        Globals.SceneManager.SetScene(sceneName);
    }
}
