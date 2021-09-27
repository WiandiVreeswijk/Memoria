using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MemoryWatchManager : MonoBehaviour {
    public const float MAX_DISTANCE = 15f;
    public const float EXECUTION_DISTANCE = 2.5f;
    public GameObject arm;
    public GameObject armRotator;
    public MemoryWatch firstPersonWatch;
    public MemoryWatch thirdPersonWatch;

    public AnimationCurve rotationCurve;
    public AnimationCurve colorCurve;
    public AnimationCurve processCurve;

    private bool armRaised = true;

    private float activationProgress = 0;
    private bool activationPressed = false;
    private bool activated = false;
    private Tween armRotationTween;


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

        Globals.Debugger.Print("a", Globals.Player.CameraController.IsInFirstPerson() + " + " + armRaised, 0.1f);
        if (Globals.Player.CameraController.IsInFirstPerson() && !armRaised) {
            RaiseArm();
        } else if (!Globals.Player.CameraController.IsInFirstPerson() && armRaised) {
            LowerArm();
        }
    }

    private void LowerArm() {
        armRotationTween?.Kill();
        armRotationTween = armRotator.transform.DOLocalRotate(new Vector3(70f, 0f, 0f), 1.0f);
        armRaised = false;
    }

    private void RaiseArm() {
        armRotationTween?.Kill();
        armRotationTween = armRotator.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 1.0f);
        armRaised = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) activationPressed = true;
        if (!Input.GetKey(KeyCode.Space)) activationPressed = false;
        if (activationPressed) {
            if (withinExecutionRangeMemoryObject != null) {
                activationProgress += processCurve.Evaluate(activationProgress) * Time.deltaTime;
            } else activationPressed = false;
        } else activationProgress -= 0.5f * Time.deltaTime;

        activationProgress = Mathf.Clamp01(activationProgress);
        thirdPersonWatch.SetWatchEdgeProgress(activationProgress, activationPressed && activationProgress < 0.99f, !Globals.Player.CameraController.IsInFirstPerson());
        firstPersonWatch.SetWatchEdgeProgress(activationProgress, activationPressed && activationProgress < 0.99f, Globals.Player.CameraController.IsInFirstPerson());

        if (!activated && activationProgress >= 0.99f && !activationPressed) {
            withinExecutionRangeMemoryObject.Activate();
            print("activate watch");
            if (Globals.Player.CameraController.IsInFirstPerson()) {
                firstPersonWatch.Activate(withinExecutionRangeMemoryObject);
            } else thirdPersonWatch.Activate(withinExecutionRangeMemoryObject);
            activated = true;
        }

        if (activationProgress < 0.1f) activated = false;
    }
}
