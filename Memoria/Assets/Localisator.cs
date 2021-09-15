using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localisator : MonoBehaviour
{
    public void OnValueChange(int value)
    {
        print(value);
    }
    public void SetDutch()
    {
        DialogueManager.SetLanguage("nl"); 
    }

    public void SetEnglish()
    {
        DialogueManager.SetLanguage("");
    }
}
