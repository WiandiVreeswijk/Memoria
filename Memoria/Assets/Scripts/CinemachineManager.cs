using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using static Cinemachine.CinemachineBrain;

public class CinemachineManager : MonoBehaviour {
    CinemachineBrain[] brains;
    public CinemachineBlenderSettings blends;
    private CinemachineBlendDefinition noBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
    private bool useBlending = true;
    private bool inputEnabled = true;

    void Awake()
    {
        brains = FindObjectsOfType<CinemachineBrain>();
        CinemachineCore.GetInputAxis = CinemachineAxisInputDelegate;
        CinemachineCore.GetBlendOverride = GetBlendOverrideDelegate;
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

    public void ClearNextBlend() {
        useBlending = false;
    }


    public CinemachineBlendDefinition GetBlendOverrideDelegate(ICinemachineCamera fromVcam, ICinemachineCamera toVcam, CinemachineBlendDefinition defaultBlend, MonoBehaviour owner) {
        if (useBlending) {
            useBlending = false;
            return blends.GetBlendForVirtualCameras(fromVcam.Name, toVcam.Name, noBlend);
        }
        return noBlend;
    }
}
