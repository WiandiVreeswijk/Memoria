using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGemCollectible : MonoBehaviour, IEnterActivatable
{
    public void ActivateEnter()
    {
        string localized = Globals.Localization.Get("INTR_GEM");
        Globals.UIManager.NotificationManager.NotifyPlayerBig(localized, () => { });
    }
}
