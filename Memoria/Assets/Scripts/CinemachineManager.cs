using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CinemachineManager : MonoBehaviour {
    CinemachineBrain[] brains;
    public CinemachineBlenderSettings blends;
    private CinemachineBlendDefinition noBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
    private bool useBlending = true;
    private bool inputEnabled = true;

    void Awake() {
        brains = FindObjectsOfType<CinemachineBrain>();
        CinemachineCore.GetInputAxis = CinemachineAxisInputDelegate;
        CinemachineCore.GetBlendOverride = GetBlendOverrideDelegate;
    }

    public void SetInputEnabled(bool toggle) {
        inputEnabled = toggle;
    }

    public float CinemachineAxisInputDelegate(string axisName)
    {
        if (inputEnabled) return Input.GetAxis(axisName);
        return 0;
    }

    public void ClearNextBlend() {
        useBlending = false;
    }


    public CinemachineBlendDefinition GetBlendOverrideDelegate(ICinemachineCamera fromVcam, ICinemachineCamera toVcam, CinemachineBlendDefinition defaultBlend, MonoBehaviour owner) {
        if (useBlending) {
            useBlending = true;
            return blends.GetBlendForVirtualCameras(fromVcam.Name, toVcam.Name, defaultBlend);
        }
        return noBlend;
    }
}
