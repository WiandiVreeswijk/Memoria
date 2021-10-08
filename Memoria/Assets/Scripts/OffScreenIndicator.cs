using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenIndicator : MonoBehaviour {
    public GameObject target;

    [SerializeField] private GameObject indicatorObject;
    [SerializeField] private GameObject indicatorCenterObject;
    public float screenBoundOffset = 0.9f;

    void Update() {
        if (!target.activeInHierarchy) {
            indicatorObject.SetActive(false);
            indicatorCenterObject.SetActive(false);
            return;
        }

        Vector3 screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        Vector3 screenBounds = screenCentre * screenBoundOffset;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
        bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width &&
                               screenPosition.y > 0 && screenPosition.y < Screen.height;

        if (isTargetVisible) {
            screenPosition.z = 0;
            indicatorObject.SetActive(false);
            indicatorCenterObject.SetActive(false);
        } else {
            indicatorObject.SetActive(true);
            indicatorCenterObject.SetActive(true);
            float angle = float.MinValue;
            GetIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds);
            indicatorCenterObject.transform.position = screenPosition;
            indicatorObject.transform.position = screenPosition;
            indicatorObject.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
    }

    //https://github.com/jinincarnate/off-screen-indicator
    public static void GetIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds) {
        screenPosition -= screenCentre;

        if (screenPosition.z < 0) screenPosition *= -1;

        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        float slope = Mathf.Tan(angle);

        if (screenPosition.x > 0) {
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        } else {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }
        if (screenPosition.y > screenBounds.y) {
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        } else if (screenPosition.y < -screenBounds.y) {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }
        screenPosition += screenCentre;
    }
}
