using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamworksManager : MonoBehaviour {
    void Awake() {
        try {
            Steamworks.SteamClient.Init(1791770);
        } catch (System.Exception e)
        {
            throw e;
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
    }

    void Update() {
        Steamworks.SteamClient.RunCallbacks();
    }

    public void OnDisable() {
        Steamworks.SteamClient.Shutdown();
    }
}
