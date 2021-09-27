using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchCollectible : MonoBehaviour, IEnterActivatable {
    void Start() {
        gameObject.SetActive(false);
    }

    public void ActivateEnter() {
        Globals.ProgressionManager.CollectWatch();
        gameObject.SetActive(false);
    }
}
