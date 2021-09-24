using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {
    private Icon questIcon;
    private bool watchCollected = false;


    void Start() {
        if (Globals.SceneManager.GetActiveScene().name == "Wijk") { //#TODO: This is temporary
            GameObject manfred = GameObject.Find("Neighbour");
            questIcon = Globals.IconManager.AddWorldIcon("oma", manfred.transform.position + new Vector3(0, 2.25f, 0));
        }
    }

    public void CollectWatch() {
        watchCollected = true;
    }

    public Icon GetIcon() {
        return questIcon;
    }
}