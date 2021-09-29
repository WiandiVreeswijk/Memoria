using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {
    private Icon questIcon;
    private bool watchCollected = false;

    public void CollectWatch() {
        watchCollected = true;
        Globals.MemoryWatchManager.EnableMemoryWatch();
    }

    public Icon GetIcon() {
        return questIcon;
    }

    public void OnGlobalsInitializeType(Globals.GlobalsType currentGlobalsType) {
        if (currentGlobalsType == Globals.GlobalsType.NEIGHBORHOOD) { //#TODO: This is temporary
            GameObject manfred = Utils.FindUniqueObject<Manfred>().gameObject;
            questIcon = Globals.IconManager.AddWorldIcon("oma", manfred.transform.position + new Vector3(0, 2.25f, 0));
        } else {
            if (questIcon != null) questIcon.SetEnabled(false);
        }
    }
}