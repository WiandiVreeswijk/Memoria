using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChasingMusic : MonoBehaviour {
    void Start() {
        Globals.SoundManagerChase.FadeMusicVolume(1.0f);
        Globals.SoundManagerChase.FadeAmbientVolume(1.0f);
    }
}
