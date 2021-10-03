using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuCamera : MonoBehaviour {
    public CinemachineVirtualCamera cam;

    [Serializable]
    public class MenuCameraPoint {
        public string name = "";
        public Vector3 position;
        public Quaternion rotation;

        public MenuCameraPoint(Vector3 position, Quaternion rotation) {
            this.position = position;
            this.rotation = rotation;
        }
    }

    public List<MenuCameraPoint> menuCameraPoints = new List<MenuCameraPoint>();

    void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();
        MenuCameraPoint randomPoint = menuCameraPoints[Random.Range(0, menuCameraPoints.Count)];
        cam.transform.position = randomPoint.position;
        cam.transform.rotation = randomPoint.rotation;
    }

    //private void Update() {
    //    if (Input.GetKeyDown(KeyCode.Space)) {
    //        cam = GetComponent<CinemachineVirtualCamera>();
    //        MenuCameraPoint randomPoint = menuCameraPoints[Random.Range(0, menuCameraPoints.Count)];
    //        cam.transform.position = randomPoint.position;
    //        cam.transform.rotation = randomPoint.rotation;
    //    }
    //}
}
