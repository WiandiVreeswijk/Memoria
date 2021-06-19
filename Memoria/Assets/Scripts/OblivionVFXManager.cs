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
        SetPosition();
    }

    private void SetPosition() {
        foreach (var vfx in oblivionParticles) {
            var temp = vfx.transform.position;
            temp.x = oblivion.GetOblivionPosition() - offset;
            vfx.transform.position = temp;
        }
    }

    public void ClearParticles() {
        SetPosition();
        foreach (var vfx in oblivionParticles) {
            vfx.Reinit();
        }
    }
}
