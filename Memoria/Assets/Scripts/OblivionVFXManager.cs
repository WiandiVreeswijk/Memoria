using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OblivionVFXManager : MonoBehaviour {
    private OblivionManager oblivion;
    private VisualEffect[] oblivionParticles;
    public float offset = 5.0f;

    private void Start() {
        oblivion = GetComponent<OblivionManager>();
        oblivionParticles = GetComponentsInChildren<VisualEffect>();
    }

    private void FixedUpdate() {
        foreach (var fvx in oblivionParticles) {
            var temp = fvx.transform.position;
            temp.x = oblivion.GetOblivionPosition() - offset;
            fvx.transform.position = temp;
        }
    }
}
