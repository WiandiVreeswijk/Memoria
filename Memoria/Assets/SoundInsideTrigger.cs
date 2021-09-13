using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInsideTrigger : MonoBehaviour
{
    private bool inside = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!inside)
        {
            Globals.SoundManagerWijk.FadeRainStatus(1, 1f);
            inside = true;
        }
        else
        {
            Globals.SoundManagerWijk.FadeRainStatus(0, 1f);
            inside = false;
        }
    }
}
