using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortalActivatable : MonoBehaviour, IEnterActivatable {
    private ParticleSystem pSystem;
    public GameObject trigger;
    void Start() {
        pSystem = GetComponent<ParticleSystem>();
    }

    public void ActivateEnter() {
        pSystem.Play();
        Utils.DelayedAction(1.0f, () => trigger.SetActive(true));
    }
}
