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
    /*
    private Dictionary<string, Queue<float>> dict = new Dictionary<string, Queue<float>>();
    public float CinemachineAxisInputDelegate(string axisName) {
        if (!dict.ContainsKey(axisName)) {
            dict.Add(axisName, new Queue<float>());
        }

        if (inputEnabled) {
            float newVal = Input.GetAxis(axisName);
            dict[axisName].Enqueue(newVal);
            if (dict[axisName].Count > 5) dict[axisName].Dequeue();
            float endVal = 0;
            foreach (float val in dict[axisName]) {
                endVal += val;
            }

            endVal /= dict[axisName].Count;
            return endVal;
        }
        return 0;
    }
    */
    public float CinemachineAxisInputDelegate(string axisName) {
        //float multiplier = 1.0f;
        //if (inputEnabled) {
        //    switch (axisName) {
        //        case "CameraControl Y FPS": multiplier = 0.5f; break;
        //        case "CameraControl X FPS": multiplier = 0.5f; break;
        //    }
        //    return Input.GetAxis(axisName) * multiplier;
        //}
        return Input.GetAxis(axisName);
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
