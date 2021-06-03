using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCollectible : MonoBehaviour {
    Renderer render;

    private void Start() {
        render = GetComponent<Renderer>();
    }
    private void FixedUpdate() {
        render.transform.Rotate(new Vector3(0, 1.0f, 0));
    }
}
