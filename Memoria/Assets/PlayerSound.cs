using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private float distance = 0.1f;
    private float material;

    public LayerMask ground;

    private void FixedUpdate()
    {
        MaterialCheck();
        Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
    }

    private void MaterialCheck()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, ground);

        if (hit.collider)
        {
            if (hit.collider.tag == "Material: Wood")
                material = 1f;
            else if (hit.collider.tag == "Material: Stone")
                material = 2f;
            else
                material = 1f;
        }
    }

    private void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByName("Material", material);
        Footsteps.start();
        Footsteps.release();
    }
}
