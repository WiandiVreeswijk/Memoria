using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionAudio : MonoBehaviour
{
    [Header("FMOD Event")]
    [FMODUnity.EventRef]
    public string SelectAudio;
    FMOD.Studio.EventInstance Audio;
    FMOD.Studio.PARAMETER_ID VolumeParameter;
    FMOD.Studio.PARAMETER_ID LPFParameter;

    Transform SlLocation;

    [Header("Occlusion Options")]
    [Range(0f, 1f)]
    public float VolumeValue = 1f;
    [Range(0f, 1f)]
    public float LPFCutoff = 1f;
    public LayerMask OcclusionLayer = 1;

    void Awake()
    {
        SlLocation = GameObject.Find("POVPoint").transform;
        //SlLocation = GameObject.FindObjectOfType<StudioListener>().transform;
        Audio = FMODUnity.RuntimeManager.CreateInstance(SelectAudio);

        FMOD.Studio.EventDescription volumeDescription;
        Audio.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("Volume", out volumeParameterDescription);
        VolumeParameter = volumeParameterDescription.id;

        FMOD.Studio.EventDescription lowpassDescription;
        Audio.getDescription(out lowpassDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION lowpassParameterDescription;
        lowpassDescription.getParameterDescriptionByName("LPF", out lowpassParameterDescription);
        LPFParameter = lowpassParameterDescription.id;
    }

    void Start()
    {
        FMOD.Studio.PLAYBACK_STATE PbState;
        Audio.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            Audio.start();
        }
    }

    void FixedUpdate()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Audio, GetComponent<Transform>(), GetComponent<Rigidbody>());

        bool playerHit = false;
        RaycastHit hit;
        if (Physics.Linecast(transform.position, SlLocation.position, out hit, OcclusionLayer))
        {
            if (hit.collider.tag == "Player")
            {
                playerHit = true;
                //Debug.Log ("not occluded");
                NotOccluded();
                Debug.DrawLine(transform.position, SlLocation.position, Color.green);
            }
            else Debug.DrawLine(transform.position, hit.point, Color.blue);
        }
        if(!playerHit)
        {
            //Debug.Log ("occluded");
            Occluded();
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }

    void Occluded()
    {
        Audio.setParameterByID(VolumeParameter, VolumeValue);
        Audio.setParameterByID(LPFParameter, LPFCutoff);
    }

    void NotOccluded()
    {
        Audio.setParameterByID(VolumeParameter, 1);
        Audio.setParameterByID(LPFParameter, 1);
    }
}
