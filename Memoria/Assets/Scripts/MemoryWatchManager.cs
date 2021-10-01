using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MemoryWatchManager : MonoBehaviour {
    private const float MAX_DISTANCE = 5.5f;
    private const float MIN_ARM_RAISE_DISTANCE = 5.0f;
    private const float EXECUTION_DISTANCE = 2.5f;
    private const float ARM_ROTATION = 70f;

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
    public string soundPath;


    MemoryObject[] memoryObjects;
    private MemoryObject withinExecutionRangeMemoryObject;

    void Start() {
        memoryObjects = FindObjectsOfType<MemoryObject>();
        foreach (MemoryObject mo in memoryObjects) mo.UpdateDistance(float.MaxValue);
        DisableMemoryWatch();

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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) activationPressed = true;
        if (!Input.GetKey(KeyCode.Space)) activationPressed = false;
        bool canActivate = activationProgress >= 0.99f;
        if (activated)
        {
            activationProgress = 1.0f;
        }
        else
        {
            if (activationPressed)
            {
                if (!soundIsPlaying)
                {
                    charge.start();
                    soundIsPlaying = true;
                }
                if (withinExecutionRangeMemoryObject != null)
                {
                    activationProgress += processCurve.Evaluate(activationProgress) * Time.deltaTime *
                                          watchActivationSpeed;
                }
                else activationPressed = false;
            }
            else
            {
                if (soundIsPlaying)
                {
                    //Stop sound here
                    charge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    soundIsPlaying = false;
                }
                activationProgress -= 0.5f * Time.deltaTime;
            }
        }

        activationProgress = Mathf.Clamp01(activationProgress);
        //thirdPersonWatch.SetWatchEdgeProgress(activationProgress, activationPressed && activationProgress < 0.99f, !Globals.Player.CameraController.IsInFirstPerson());
        firstPersonWatch.SetWatchEdgeProgress(activationProgress, activationPressed, activationProgress >= 0.99f && activationPressed, Globals.Player.CameraController.IsInFirstPerson());

        if (!activated && canActivate && !activationPressed) {
            withinExecutionRangeMemoryObject.Activate();
            print("activate watch");
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
