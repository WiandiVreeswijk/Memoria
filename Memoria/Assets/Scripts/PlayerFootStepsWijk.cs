using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepsWijk : MonoBehaviour {
    private float distance = 0.1f;
    private float material;

    public LayerMask ground;

    private void FixedUpdate() {
        MaterialCheck();
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.blue);
    }

    private void MaterialCheck() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distance, ground)) {
            if (hit.collider.tag == "Material: Grass") {
                material = 1f;
            } else if (hit.collider.tag == "Material: Stone") {
                material = 2f;
            } else if (hit.collider.tag == "Material: Wood") {
                material = 3f;
            } else {
                material = 1f;
            }
        }
    }

    public void PlayFootstepsEvent(string path) {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByName("FootstepsMaterialWijk", material);
        Footsteps.start();
        Footsteps.release();
    }
}
