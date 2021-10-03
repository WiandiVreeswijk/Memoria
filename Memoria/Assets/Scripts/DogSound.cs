using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSound : MonoBehaviour
{
    private float distance = 0.1f;
    private float material;

    public LayerMask ground;

    //FMOD
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    public new FMOD.Studio.EventInstance audio;
    private PARAMETER_ID volumeParameter;

    [Header("dog Volume")]
    [Range(0f, 1f)]
    public float volumeValue = 0f;
    public float volumeMultiplier;

    private void Start()
    {
        audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("DogVolume", out volumeParameterDescription);
        volumeParameter = volumeParameterDescription.id;


    }
    private void FixedUpdate()
    {
        MaterialCheck();
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.blue);

        float distanceToPlayer = Vector3.Distance(Globals.Player.transform.position, transform.position);
        volumeValue = Mathf.Lerp(1.0f, 0, distanceToPlayer * volumeMultiplier);
    }

    private void MaterialCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distance, ground))
        {
            if (hit.collider.tag == "Material: Grass")
            {
                material = 1f;
            }
            else if (hit.collider.tag == "Material: Stone")
            {
                material = 2f;
            }
            else
            {
                material = 1f;
            }
        }
    }

    public void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByID(volumeParameter, volumeValue);
        Footsteps.setParameterByName("MaterialDog", material);
        Footsteps.start();
        Footsteps.release();
    }

    public void PlayBarkEvent(string path)
    {
        FMOD.Studio.EventInstance Bark = FMODUnity.RuntimeManager.CreateInstance(path);
        Bark.setParameterByID(volumeParameter, volumeValue);
        Bark.start();
        Bark.release();
    }
}
