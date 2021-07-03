using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CollectiblePlacer : EditorWindow {
    [Serializable]
    private class Point : Object {
        public bool isTangent;
        public Vector3 point;
    }

    private GameObject prefab;

    private Point start;
    private Point startTangent;
    private Point end;
    private Point endTangent;

    private float spaceBetweenPoints = 1.0f;
    List<Vector3> pointsOnCurve = new List<Vector3>();

    [MenuItem("WAEM/Collectible placer")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(CollectiblePlacer));
    }

    public void Initialize() {
        InitializePoint(ref start, false, new Vector3(-1.0f, 0.0f, 0.0f));
        InitializePoint(ref startTangent, true, new Vector3(-1.0f, 1.0f, 0.0f));
        InitializePoint(ref end, false, new Vector3(1.0f, 0.0f, 0.0f));
        InitializePoint(ref endTangent, true, new Vector3(1.0f, 1.0f, 0.0f));
        GeneratePointsOnCurve();
    }

    public void OnEnable() {
        Initialize();
        var assets = AssetDatabase.FindAssets("GemCollectible");
        foreach (string asset in assets) {
            string path = AssetDatabase.GUIDToAssetPath(asset);
            if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(GameObject)) {
                prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            }
        }

        SceneView.duringSceneGui += DuringSceneGui;
    }

    void OnDestroy() {
        SceneView.duringSceneGui -= DuringSceneGui;
    }

    private void DuringSceneGui(SceneView obj) {
        bool dirty = false;
        dirty |= MovePoint(ref start);
        dirty |= MovePoint(ref startTangent);
        dirty |= MovePoint(ref end);
        dirty |= MovePoint(ref endTangent);
        if (dirty) GeneratePointsOnCurve();
        Handles.DrawBezier(start.point, end.point, startTangent.point, endTangent.point, Color.green, null, 2.0f);
        Handles.color = Color.white;
        Handles.DrawLine(start.point, startTangent.point);
        Handles.DrawLine(end.point, endTangent.point);



        foreach (var point in pointsOnCurve) {
            Handles.DrawWireCube(point, new Vector3(0.25f, 0.25f, 0.25f));
        }

        //for (float t = 0; t <= 1; t += 0.05f) {
        //    Vector3 gizmosPos = Mathf.Pow(1 - t, 3) * start.point +
        //                        3 * Mathf.Pow(1 - t, 2) * t * startTangent.point +
        //                        3 * (1 - t) * Mathf.Pow(t, 2) * endTangent.point +
        //                        Mathf.Pow(t, 3) * end.point;
        //
        //    Handles.DrawWireCube(gizmosPos, new Vector3(0.25f, 0.25f, 0.25f));
        //}
    }

    private void GeneratePointsOnCurve() {
        pointsOnCurve.Clear();
        pointsOnCurve.Add(start.point);
        Vector3 previousPoint = start.point;
        float dstSinceLastEventPoint = 0;
        float t = 0;
        while (t <= 1) {
            t += 0.1f;
            Vector3 pointOnCurve = Utils.EvaluateCubic(start.point, startTangent.point, endTangent.point, end.point, t);
            dstSinceLastEventPoint += Vector3.Distance(previousPoint, pointOnCurve);
            while (dstSinceLastEventPoint >= spaceBetweenPoints) {
                float overshootDst = dstSinceLastEventPoint - spaceBetweenPoints;
                Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                pointsOnCurve.Add(newEvenlySpacedPoint);
                dstSinceLastEventPoint = overshootDst;
                previousPoint = newEvenlySpacedPoint;
                if (pointsOnCurve.Count > 200) return;
            }

            previousPoint = pointOnCurve;
        }
    }

    private void InitializePoint(ref Point point, bool isTangent, Vector3 offset) {
        point = new Point();
        point.point = SceneView.lastActiveSceneView.camera.transform.TransformPoint(Vector3.forward * -SceneView.lastActiveSceneView.camera.transform.position.z);
        point.point.z = 0.0f;
        point.point += offset;
        point.isTangent = isTangent;
    }

    private bool MovePoint(ref Point point) {
        Handles.color = point.isTangent ? Color.yellow : Color.red;
        Vector3 newPos = Handles.FreeMoveHandle(point.point, Quaternion.identity, 0.1f, new Vector3(0, 0, 99999999999), Handles.SphereHandleCap);
        if (point.point != newPos) {
            point.point = newPos;
            point.point.z = 0.0f;
            return true;
        }
        return false;
    }

    public void OnGUI() {
        prefab = EditorGUILayout.ObjectField("collectiblePrefab", prefab, typeof(GameObject), false) as GameObject;


        if (GUILayout.Button("Reset curve position in front of camera")) {
            Initialize();
        }

        GUILayout.Space(10.0f);

        EditorGUI.BeginChangeCheck();
        spaceBetweenPoints = EditorGUILayout.Slider(spaceBetweenPoints, 0.01f, 10.0f);
        if (EditorGUI.EndChangeCheck()) {
            GeneratePointsOnCurve();
        }

        if (GUILayout.Button("Place Points")) {
            foreach (var point in pointsOnCurve) {
                GameObject obj = Instantiate(prefab, point, Quaternion.identity);
                Undo.RegisterCreatedObjectUndo(obj, "CreatedPoint");
            }
        }
    }
}
