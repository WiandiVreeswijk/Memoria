using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryWatchManager : MonoBehaviour {
    public const float MAX_DISTANCE = 15f;
    public const float EXECUTION_DISTANCE = 2.5f;
    public MemoryWatch firstPersonWatch;
    public MemoryWatch thirdPersonWatch;

    public AnimationCurve rotationCurve;
    public AnimationCurve colorCurve;
    public AnimationCurve processCurve;


    MemoryObject[] memoryObjects;
    private MemoryObject withinExecutionRangeMemoryObject;
    void Start() {
        memoryObjects = FindObjectsOfType<MemoryObject>();
    }

    void FixedUpdate() {
        MemoryObject closest = null;
        float closestDistance = float.MaxValue;
        foreach (MemoryObject mo in memoryObjects) {
            float distance = Vector3.Distance(mo.transform.position, Globals.Player.transform.position);
            if (distance < MAX_DISTANCE && distance < closestDistance) {
                closest = mo;
                closestDistance = distance;
            }
        }

        if (closest != null) {
            if (Globals.Player.CameraController.IsInFirstPerson()) {
                firstPersonWatch.SetActivity(1.0f - (closestDistance / MAX_DISTANCE), closest);
                thirdPersonWatch.SetActivity(0, null);
            } else {
                firstPersonWatch.SetActivity(0, null);
                thirdPersonWatch.SetActivity(1.0f - (closestDistance / MAX_DISTANCE), closest);
            }
        }

        if (closestDistance < EXECUTION_DISTANCE) {
            withinExecutionRangeMemoryObject = closest;
        } else withinExecutionRangeMemoryObject = null;
    }

    private float progress = 0;
    private bool pressed = false;
    private bool activated = false;
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) pressed = true;
        if (!Input.GetKey(KeyCode.Space)) pressed = false;
        if (pressed) {
            if (withinExecutionRangeMemoryObject != null) {
                progress += processCurve.Evaluate(progress) * Time.deltaTime;
            } else pressed = false;
        } else progress -= 0.5f * Time.deltaTime;

        progress = Mathf.Clamp01(progress);
        thirdPersonWatch.SetWatchEdgeProgress(progress, pressed && progress < 0.99f, !Globals.Player.CameraController.IsInFirstPerson());
        firstPersonWatch.SetWatchEdgeProgress(progress, pressed && progress < 0.99f, Globals.Player.CameraController.IsInFirstPerson());

        if (!activated && progress >= 0.99f && !pressed) {
            withinExecutionRangeMemoryObject.Activate();
            if (Globals.Player.CameraController.IsInFirstPerson()) {
                firstPersonWatch.Activate(withinExecutionRangeMemoryObject);
            } else thirdPersonWatch.Activate(withinExecutionRangeMemoryObject);
            activated = true;
        }

        if (progress < 0.1f) activated = false;
    }
}
