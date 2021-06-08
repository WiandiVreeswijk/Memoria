using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OblivionVFXManager : MonoBehaviour
{
    private OblivionManager oblivion;
    private VisualEffect oblivionParticles;
    private float offset = 10f;

    private void Start()
    {
        oblivion = GetComponent<OblivionManager>();
        oblivionParticles = GetComponentInChildren<VisualEffect>();
    }
    private void FixedUpdate()
    {
        var temp = oblivionParticles.transform.position;
        temp.x = oblivion.GetOblivionPosition() - offset;
        oblivionParticles.transform.position = temp;
    }
}
