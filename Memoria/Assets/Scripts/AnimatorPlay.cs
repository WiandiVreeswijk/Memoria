using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlay : MonoBehaviour {
    void Start() {
        GetComponent<Animator>().Play("GemAnimation", 0);
    }
}
