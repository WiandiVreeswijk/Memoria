using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePlacer : MonoBehaviour {
    [Serializable]
    public struct Point {
        [SerializeField] public bool isTangent;
        [SerializeField] public Vector3 point;

        public Point(Vector3 point, bool isTangent)
        {
            this.point = point;
            this.isTangent = isTangent;
        }
    }

    [HideInInspector] public GameObject prefab;
    [Range(0.01f, 10.0f)] public float spaceBetweenPoints = 0.75f;

    [HideInInspector] public Point start = new Point(new Vector3(-1.0f, 0.0f, 0.0f), false);
    [HideInInspector] public Point startTangent = new Point(new Vector3(-1.0f, 1.0f, 0.0f), true);
    [HideInInspector] public Point end = new Point(new Vector3(1.0f, 0.0f, 0.0f), false);
    [HideInInspector] public Point endTangent = new Point(new Vector3(1.0f, 1.0f, 0.0f), true);

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
