using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private float distance = 0.1f;
    private float material;
    private float mute;

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
            {
                mute = 0f;
                material = 1f;
            }
            else if (hit.collider.tag == "Material: Stone")
            {
                mute = 0f;
                material = 2f;
            }
            else
            {
                material = 1f;
                mute = 1f;
            }
        }
    }

    private void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByName("Material", material);
        Footsteps.start();
        Footsteps.release();
    }

    public void PlayJumpSound()
    {
        FMOD.Studio.EventInstance jump = FMODUnity.RuntimeManager.CreateInstance("event:/SFXChasingLevel/Player/JumpLanding");
        jump.setParameterByName("Mute", mute);
        jump.setParameterByName("MaterialJump", material);
        jump.start();
        jump.release();
    }
}
