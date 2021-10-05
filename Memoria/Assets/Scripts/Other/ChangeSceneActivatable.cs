using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneActivatable : MonoBehaviour, IEnterActivatable {
    public string sceneName;

    private waterfallSound waterfallSound;

    private void Start()
    {
        waterfallSound = GetComponent<waterfallSound>();
    }

    public void ActivateEnter() {
        Globals.SoundManagerChase.FadeMusicVolume(0.0f);
        Globals.SoundManagerChase.FadeAmbientVolume(0.0f);
        waterfallSound.MuteWaterfall();
        Globals.Player.PlayerMovement25D.SetStunned(true, true, true);
        Globals.SceneManager.SetScene(sceneName);
    }
}
