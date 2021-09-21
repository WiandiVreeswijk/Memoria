using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public void PlayHoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFXUI/hoverUI");
    }
    
    public void PlaySelectedSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFXUI/selectedUI");
    }
}
