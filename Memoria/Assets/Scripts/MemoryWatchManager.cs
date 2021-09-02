using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryWatchManager : MonoBehaviour
{
    public const int MAX_DISTANCE = 15;
    public MemoryWatch firstPersonWatch;
    public MemoryWatch thirdPersonWatch;

    public AnimationCurve rotationCurve;
    public AnimationCurve colorCurve;

    MemoryObject[] memoryObjects;

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
    }
}
