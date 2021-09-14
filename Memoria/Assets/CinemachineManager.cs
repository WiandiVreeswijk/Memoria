using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static Cinemachine.CinemachineBrain;

public class CinemachineManager : MonoBehaviour {
    CinemachineBrain[] brains;

    void Start() {
        brains = FindObjectsOfType<CinemachineBrain>();
    }

    public void SetPausedState(bool toggle) {
        foreach (var brain in brains) {
            brain.m_UpdateMethod = toggle ? UpdateMethod.FixedUpdate : UpdateMethod.SmartUpdate;
            brain.m_BlendUpdateMethod = toggle ? BrainUpdateMethod.FixedUpdate : BrainUpdateMethod.LateUpdate;
        }
    }
}
