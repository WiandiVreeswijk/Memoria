using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteamworksManager : MonoBehaviour {
    void Awake() {
        try {
            Steamworks.SteamClient.Init(1791770);
        } catch (System.Exception e) {
            print(e.Message);
            //throw e;
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) Steamworks.SteamUserStats.IndicateAchievementProgress("Fiets", 1, 100);
        //foreach (var c in Steamworks.SteamInput.Controllers) {
        //    print(c.Id);
        //}
        Steamworks.SteamClient.RunCallbacks();
    }

    public void OnDisable() {
        Steamworks.SteamClient.Shutdown();
    }
}
