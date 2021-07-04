using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CollectiblePlacer : EditorWindow {
    [Serializable]
    private class Point : Object {
        [SerializeField] public bool isTangent;
        [SerializeField] public Vector3 point;
    }

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
                     
    [SerializeField] private Point start;
    [SerializeField] private Point startTangent;
    [SerializeField] private Point end;
    [SerializeField] private Point endTangent;

    private float spaceBetweenPoints = 1.0f;
    List<Vector3> pointsOnCurve = new List<Vector3>();

    [MenuItem("WAEM/Collectible placer")]
    public static void ShowWindow() {
        CollectiblePlacer window = (CollectiblePlacer)EditorWindow.GetWindowWithRect(typeof(CollectiblePlacer), new Rect(0, 0, 300, 190));
    }

    public void Initialize(bool staight) {
        InitializePoint(ref start, false, new Vector3(-1.0f, 0.0f, 0.0f));
        InitializePoint(ref startTangent, true, new Vector3(staight ? -0.1f : -1.0f, staight ? 0.0f : 1.0f, 0.0f));
        InitializePoint(ref end, false, new Vector3(1.0f, 0.0f, 0.0f));
        InitializePoint(ref endTangent, true, new Vector3(staight ? 0.1f : 1.0f, staight ? 0.0f : 1.0f));
        GeneratePointsOnCurve();
    }

    public void OnEnable() {
        Initialize(false);
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
        dirty |= MoveAll();
        if (dirty) GeneratePointsOnCurve();
        Bezier(start.point, end.point, startTangent.point, endTangent.point, Color.green);
        Line(start.point, startTangent.point, Color.white);
        Line(end.point, endTangent.point, Color.white);

        foreach (var point in pointsOnCurve) {
            Handles.DrawWireCube(point, new Vector3(0.25f, 0.25f, 0.25f));
        }
    }

    private void GeneratePointsOnCurve() {
        pointsOnCurve.Clear();
        pointsOnCurve.Add(start.point);
        Vector3 previousPoint = start.point;
        float dstSinceLastEventPoint = 0;
        float t = 0;
        while (t <= 1) {
            t += 0.025f;
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

    private void Line(Vector3 pos1, Vector3 pos2, Color color)
    {
        Handles.color = color;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawLine(pos1, pos2);
        Handles.color = Color.gray;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.DrawLine(pos1, pos2);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
    }

    private void Bezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Color color) {
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawBezier(pos1, pos2, pos3, pos4, color, null, 2.0f);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.DrawBezier(pos1, pos2, pos3, pos4, Color.gray, null, 2.0f);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
    }

    private Vector3 FreeMoveHandle(Vector3 pos, Color color) {
        Vector3 toRet;
        Handles.color = color;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        toRet = Handles.FreeMoveHandle(pos, Quaternion.identity, 0.1f, new Vector3(0.25f, 0.25f, 0), Handles.SphereHandleCap);
        Handles.color = Color.gray;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        toRet = Handles.FreeMoveHandle(pos, Quaternion.identity, 0.1f, new Vector3(0.25f, 0.25f, 0), Handles.SphereHandleCap);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        return toRet;
    }
    private bool MoveAll() {
        Vector3 avgPos = start.point + startTangent.point + end.point + endTangent.point;
        avgPos /= 4.0f;
        Vector3 newPos = FreeMoveHandle(avgPos, Color.green);

        newPos.z = 0;
        if (avgPos != newPos) {
            start.point = start.point - avgPos + newPos;
            startTangent.point = startTangent.point - avgPos + newPos;
            end.point = end.point - avgPos + newPos;
            endTangent.point = endTangent.point - avgPos + newPos;
            return true;
        }
        return false;
    }
    private bool MovePoint(ref Point point) {
        Vector3 newPos = FreeMoveHandle(point.point, point.isTangent ? Color.yellow : Color.red);
        if (point.point != newPos) {
            point.point = newPos;
            point.point.z = 0.0f;
            return true;
        }
        return false;
    }

    public void OnGUI() {
        if (GUILayout.Button("Set curve position in front of camera", GUILayout.Height(30.0f))) Initialize(false);
        if (GUILayout.Button("Set straight position in front of camera", GUILayout.Height(30.0f))) Initialize(true);
        GUILayout.Space(10.0f);

        prefab = EditorGUILayout.ObjectField("collectiblePrefab", prefab, typeof(GameObject), false) as GameObject;
        parent = EditorGUILayout.ObjectField("collectibleParent", parent, typeof(Transform), true) as Transform;

        GUILayout.Space(10.0f);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Distance between points");
        spaceBetweenPoints = EditorGUILayout.Slider(spaceBetweenPoints, 0.01f, 10.0f);
        if (EditorGUI.EndChangeCheck()) {
            GeneratePointsOnCurve();
        }

        if (GUILayout.Button("Place Points")) {
            Undo.SetCurrentGroupName("PlacePoints");
            GameObject parentObj = new GameObject("Collectibles" + start.point);
            if (parent != null)
                parentObj.transform.parent = parent;
            foreach (var point in pointsOnCurve) {
                GameObject obj = Instantiate(prefab, point, Quaternion.identity);
                obj.name = prefab.name + point;
                obj.transform.parent = parentObj.transform.parent;
                Undo.RegisterCreatedObjectUndo(obj, "CreatedPoint");
            }
            Undo.RegisterCreatedObjectUndo(parentObj, "CreatedPointParent");
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}
