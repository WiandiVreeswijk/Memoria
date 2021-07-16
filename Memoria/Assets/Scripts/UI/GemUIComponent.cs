using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemUIComponent : MonoBehaviour {
    private void Start() {
        GetComponent<Animator>().Play("GemAnimation");
    }
    public void PlayPFX() { }
}
