using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BusStop : MonoBehaviour, IEnterActivatable {
    private CarEngine busEngine;
    private bool skip = false;

    public void Start() {
        busEngine = GetComponent<CarEngine>();
    }

    public void Skip() {
        skip = true;
    }

    public void ActivateEnter() {
        if (skip) return;
        busEngine.isBraking = true;
        Globals.MenuController.BlackScreenFadeIn(2f).OnComplete(() => {
            Utils.DelayedAction(3, () => Globals.MenuController.BlackScreenFadeOut(3f));
        });
        Utils.DelayedAction(6, () => busEngine.isBraking = false);
    }
}
