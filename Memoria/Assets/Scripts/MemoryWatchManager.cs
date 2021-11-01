using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

public class MemoryWatchManager : MonoBehaviour {
    private const float MAX_DISTANCE = 3.5f;
    private const float MIN_ARM_RAISE_DISTANCE = 3.0f;
    private const float EXECUTION_DISTANCE = 2.5f;
    private const float ARM_ROTATION = 75f;

    public GameObject arm;
    public GameObject armRotator;
    public MemoryWatch firstPersonWatch;
    public MemoryWatch thirdPersonWatch;
    public float watchActivationSpeed = 2.0f;

    public AnimationCurve rotationCurve;
    public AnimationCurve colorCurve;
    public AnimationCurve processCurve;

    private float activationProgress = 0;
    private bool activationPressed = false;
    private bool activated = false;
    private Tween armRotationTween;

    private FMOD.Studio.EventInstance charge;
    [EventRef]
    public string soundPath;


    MemoryObject[] memoryObjects;
    private MemoryObject withinExecutionRangeMemoryObject;
    public bool shouldDisable = true;

    void Start() {
        memoryObjects = FindObjectsOfType<MemoryObject>();
        foreach (MemoryObject mo in memoryObjects) mo.UpdateDistance(float.MaxValue);
        if (shouldDisable) DisableMemoryWatch();

        charge = FMODUnity.RuntimeManager.CreateInstance(soundPath);
    }

    void FixedUpdate() {
        MemoryObject closest = null;
        float closestDistance = float.MaxValue;
        foreach (MemoryObject mo in memoryObjects) {
            float distance = Vector3.Distance(mo.transform.position, Globals.Player.transform.position);
            mo.UpdateDistance(distance);
            if (distance < MAX_DISTANCE && distance < closestDistance) {
                closest = mo;
                closestDistance = distance;
            }
        }

        if (closest != null) {
            if (Globals.Player.CameraController.IsInFirstPerson()) {
                firstPersonWatch.SetActivity(1.0f - (closestDistance / MAX_DISTANCE), closest);
                //thirdPersonWatch.SetActivity(0, null);
            } else {
                firstPersonWatch.SetActivity(0, null);
                //thirdPersonWatch.SetActivity(1.0f - (closestDistance / MAX_DISTANCE), closest);
            }
        }

        if (closestDistance < EXECUTION_DISTANCE) {
            withinExecutionRangeMemoryObject = closest;
        } else withinExecutionRangeMemoryObject = null;

        float clampedDistance = Mathf.Clamp(closestDistance, MIN_ARM_RAISE_DISTANCE, MAX_DISTANCE);
        float remappedDistance = Utils.Remap(clampedDistance, MIN_ARM_RAISE_DISTANCE, MAX_DISTANCE, 0.0f, 1.0f);
        float rotation = Globals.Player.CameraController.IsInFirstPerson() ? remappedDistance * (ARM_ROTATION - 10f) + 10f : ARM_ROTATION;
        armRotationTween?.Kill();
        armRotationTween = armRotator.transform.DOLocalRotate(new Vector3(rotation, 0.0f, 0.0f), 0.5f).SetEase(Ease.Linear);

        //thirdPersonWatch.gameObject.SetActive(!Globals.Player.CameraController.IsInFirstPerson());
    }

    bool soundIsPlaying = false;
    private float time = 0;
    void Update() {
        bool buttonDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1");
        bool button = Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 1");
        if (buttonDown) {
            time = Time.time;
            activationPressed = true;
        }
        if (!button) activationPressed = false;
        bool canActivate = activationProgress >= 0.99f;
        if (activated) {
            activationProgress = 1.0f;
        } else {
            if (activationPressed) {
                if (!soundIsPlaying) {
                    charge.start();
                    charge.setTimelinePosition((int)(1500f * activationProgress));
                    soundIsPlaying = true;
                }
                if (withinExecutionRangeMemoryObject != null) {
                    activationProgress += processCurve.Evaluate(activationProgress) * Time.deltaTime * watchActivationSpeed;
                } else activationPressed = false;
            } else {
                if (soundIsPlaying) {
                    //Stop sound here
                    charge.getTimelinePosition(out int position);
                    charge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    soundIsPlaying = false;
                }
                activationProgress -= 0.5f * Time.deltaTime;
            }
        }

        //Globals.Debugger.Print("b", "" + (Time.time - time), 1.0f);

        activationProgress = Mathf.Clamp01(activationProgress);
        //thirdPersonWatch.SetWatchEdgeProgress(activationProgress, activationPressed && activationProgress < 0.99f, !Globals.Player.CameraController.IsInFirstPerson());
        firstPersonWatch.SetWatchEdgeProgress(activationProgress, activationPressed, activationProgress >= 0.99f, Globals.Player.CameraController.IsInFirstPerson());

        if (!activated && canActivate) {
            withinExecutionRangeMemoryObject.Activate();
            if (Globals.Player.CameraController.IsInFirstPerson()) {
                firstPersonWatch.Activate(withinExecutionRangeMemoryObject);
            }
            //} else thirdPersonWatch.Activate(withinExecutionRangeMemoryObject);
            activated = true;
        }

        //if (activationProgress < 0.1f) activated = false;
    }

    public void DisableMemoryWatch() {
        enabled = false;
        //thirdPersonWatch.gameObject.SetActive(false);
        armRotator.transform.localRotation = Quaternion.Euler(ARM_ROTATION, 0f, 0f);
    }
    public void EnableMemoryWatch() {
        enabled = true;
    }
}
