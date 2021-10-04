using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChasingMusic : MonoBehaviour {
    void Start() {
        Globals.SoundManagerChase.FadeVolume(1.0f);
    }
}
