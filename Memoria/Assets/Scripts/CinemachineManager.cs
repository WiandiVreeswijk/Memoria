using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using static Cinemachine.CinemachineBrain;

public class CinemachineManager : MonoBehaviour {
    CinemachineBrain[] brains;
    private bool inputEnabled = true;
    public GameObject cameraTarget;

    void Start() {
        brains = FindObjectsOfType<CinemachineBrain>();
        CinemachineCore.GetInputAxis = CinemachineAxisInputDelegate;
    }

    public void SetPausedState(bool toggle) {
        foreach (var brain in brains) {
            brain.m_UpdateMethod = toggle ? UpdateMethod.FixedUpdate : UpdateMethod.SmartUpdate;
            brain.m_BlendUpdateMethod = toggle ? BrainUpdateMethod.FixedUpdate : BrainUpdateMethod.LateUpdate;
        }
    }

    public void SetInputEnabled(bool toggle) {
        inputEnabled = toggle;
    }

    public float CinemachineAxisInputDelegate(string axisName) {
        if (inputEnabled) return Input.GetAxis(axisName);
        return 0;
    }
}
