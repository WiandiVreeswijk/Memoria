using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortalActivatable : MonoBehaviour, IEnterActivatable {
    public GameObject trigger;

    public void ActivateEnter() {
        Utils.DelayedAction(1.0f, () => trigger.SetActive(true));
    }
}
