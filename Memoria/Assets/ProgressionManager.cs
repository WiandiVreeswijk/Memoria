using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {
    private Icon questIcon;
    private bool watchCollected = false;


    void Start() {
        GameObject oma = FindObjectOfType<DialogueHandlerOma>().gameObject;
        questIcon = Globals.IconManager.AddWorldIcon("oma", oma.transform.position + new Vector3(0, 2.25f, 0));
    }

    public void CollectWatch() {
        watchCollected = true;
    }

    public Icon GetIcon() {
        return questIcon;
    }
}
