using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollectiblePlacer))]
public class CollectiblePlacerEditor : Editor {
    List<Vector3> pointsOnCurve = new List<Vector3>();
    private SerializedProperty spaceBetweenPointsProperty;
    private SerializedProperty prefabProperty;

    private enum CollectiblePlacerPreset {
        Default,
        Straight,
        Jump,
        TifaJump,
        HopDown,
        LeapDown,
        COUNT
    }

    private void GeneratePointsOnCurve() {
        CollectiblePlacer placer = target as CollectiblePlacer;

        pointsOnCurve.Clear();
        pointsOnCurve.Add(placer.start.point);
        Vector3 previousPoint = placer.start.point;
        float dstSinceLastEventPoint = 0;
        float t = 0;
        while (t <= 1) {
            t += 0.025f;
            Vector3 pointOnCurve = Utils.EvaluateCubic(placer.start.point, placer.startTangent.point, placer.endTangent.point, placer.end.point, t);
            dstSinceLastEventPoint += Vector3.Distance(previousPoint, pointOnCurve);
            while (dstSinceLastEventPoint >= placer.spaceBetweenPoints) {
                float overshootDst = dstSinceLastEventPoint - placer.spaceBetweenPoints;
                Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                pointsOnCurve.Add(newEvenlySpacedPoint);
                dstSinceLastEventPoint = overshootDst;
                previousPoint = newEvenlySpacedPoint;
                if (pointsOnCurve.Count > 200) return;
            }

            previousPoint = pointOnCurve;
        }

        PlacePoints();
    }

    public void OnEnable() {
        spaceBetweenPointsProperty = serializedObject.FindProperty("spaceBetweenPoints");
        prefabProperty = serializedObject.FindProperty("prefab");

        GeneratePointsOnCurve();
        Undo.undoRedoPerformed += OnUndo;
    }

    public void OnDisable() {
        Undo.undoRedoPerformed -= OnUndo;
    }

    private void OnUndo() {
        GeneratePointsOnCurve();
    }

    private void OnSceneGUI() {
        CollectiblePlacer placer = target as CollectiblePlacer;
        bool dirty = false;
        Undo.RecordObject(placer, "MovePoints");
        dirty |= MovePoint(ref placer.start);
        dirty |= MovePoint(ref placer.startTangent);
        dirty |= MovePoint(ref placer.end);
        dirty |= MovePoint(ref placer.endTangent);
        if (dirty) {
            GeneratePointsOnCurve();
            EditorUtility.SetDirty(placer);
        }
        Bezier(placer.start.point, placer.end.point, placer.startTangent.point, placer.endTangent.point, Color.green);
        Line(placer.start.point, placer.startTangent.point, Color.white);
        Line(placer.end.point, placer.endTangent.point, Color.white);

        foreach (var point in pointsOnCurve) {
            Handles.DrawWireCube(point + placer.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
        }
    }

    private bool MovePoint(ref CollectiblePlacer.Point point) {
        Vector3 newPos = FreeMoveHandle(point.point, point.isTangent ? Color.yellow : Color.red);
        if (point.point != newPos) {
            point.point = newPos;
            point.point.z = 0.0f;
            return true;
        }
        return false;
    }

    private void Line(Vector3 pos1, Vector3 pos2, Color color) {
        CollectiblePlacer placer = target as CollectiblePlacer;
        Handles.color = color;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawLine(pos1 + placer.transform.position, pos2 + placer.transform.position);
        Handles.color = Color.gray;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.DrawLine(pos1 + placer.transform.position, pos2 + placer.transform.position);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
    }

    private void Bezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Color color) {
        CollectiblePlacer placer = target as CollectiblePlacer;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawBezier(pos1 + placer.transform.position, pos2 + placer.transform.position, pos3 + placer.transform.position, pos4 + placer.transform.position, color, null, 2.0f);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.DrawBezier(pos1 + placer.transform.position, pos2 + placer.transform.position, pos3 + placer.transform.position, pos4 + placer.transform.position, color, null, 2.0f);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
    }

    private Vector3 FreeMoveHandle(Vector3 pos, Color color) {
        CollectiblePlacer placer = target as CollectiblePlacer;
        Vector3 toRet;
        Handles.color = color;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        toRet = Handles.FreeMoveHandle(pos + placer.transform.position, Quaternion.identity, 0.2f, new Vector3(0.25f, 0.25f, 0), Handles.SphereHandleCap) - placer.transform.position;
        Handles.color = Color.gray;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        toRet = Handles.FreeMoveHandle(pos + placer.transform.position, Quaternion.identity, 0.2f, new Vector3(0.25f, 0.25f, 0), Handles.SphereHandleCap) - placer.transform.position;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        return toRet;
    }

    private void PlacePoints() {
        CollectiblePlacer placer = target as CollectiblePlacer;
        while (placer.transform.childCount > 0) {
            GameObject child = placer.transform.GetChild(0).gameObject;
            if (child.name.StartsWith("_collectible_")) DestroyImmediate(child);
        }
        if (placer.prefab != null) {
            foreach (var point in pointsOnCurve) {
                GameObject obj = PrefabUtility.InstantiatePrefab(placer.prefab) as GameObject;
                obj.transform.position = point + placer.transform.position;
                obj.name = "_collectible_" + placer.prefab.name + point;
                obj.transform.parent = placer.transform;
            }
        }
    }

    public override void OnInspectorGUI() {
        CollectiblePlacer placer = target as CollectiblePlacer;
        bool dirty = false;
        Undo.RecordObject(placer, "InspectorMovePoints");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(prefabProperty);
        placer.start.point = EditorGUILayout.Vector3Field("Start", placer.start.point);
        placer.startTangent.point = EditorGUILayout.Vector3Field("Start tangent", placer.startTangent.point);
        placer.end.point = EditorGUILayout.Vector3Field("End", placer.end.point);
        placer.endTangent.point = EditorGUILayout.Vector3Field("End tangent", placer.endTangent.point);
        EditorGUILayout.PropertyField(spaceBetweenPointsProperty);
        EditorGUILayout.LabelField("Presets", EditorStyles.boldLabel);

        bool begun = false;
        for (int i = 0; i < (int)CollectiblePlacerPreset.COUNT; i++) {
            if (i % 2 == 0) {
                GUILayout.BeginHorizontal();
                begun = true;
            }
            if (GUILayout.Button(ObjectNames.NicifyVariableName(((CollectiblePlacerPreset)i).ToString()), GUILayout.Height(30))) SetPreset((CollectiblePlacerPreset)i);
            if (i % 2 == 1) {
                GUILayout.EndHorizontal();
                begun = false;
            }
        }
        if (begun) GUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck()) dirty = true;

        serializedObject.ApplyModifiedProperties();
        if (dirty) GeneratePointsOnCurve();
    }

    private void SetPreset(CollectiblePlacerPreset preset) {
        CollectiblePlacer placer = target as CollectiblePlacer;
        switch (preset) {
            case CollectiblePlacerPreset.Default: {
                    placer.start.point = new Vector3(-2.0f, 0.0f, 0.0f);
                    placer.startTangent.point = new Vector3(-2.0f, 2.0f, 0.0f);
                    placer.end.point = new Vector3(2.0f, 0.0f, 0.0f);
                    placer.endTangent.point = new Vector3(2.0f, 2.0f, 0.0f);
                    placer.spaceBetweenPoints = 0.75f;
                }
                break;
            case CollectiblePlacerPreset.Straight: {
                    placer.start.point = new Vector3(-2.0f, 0.0f, 0.0f);
                    placer.startTangent.point = new Vector3(-0.2f, 0.0f, 0.0f);
                    placer.end.point = new Vector3(2.0f, 0.0f, 0.0f);
                    placer.endTangent.point = new Vector3(0.2f, 0.0f, 0.0f);
                    placer.spaceBetweenPoints = 0.75f;
                }
                break;
            case CollectiblePlacerPreset.Jump: {
                    placer.start.point = new Vector3(-2.0f, 1.0f, 0.0f);
                    placer.startTangent.point = new Vector3(-1.2f, 3.0f, 0.0f);
                    placer.end.point = new Vector3(2.0f, 2.5f, 0.0f);
                    placer.endTangent.point = new Vector3(0.5f, 5.0f, 0.0f);
                    placer.spaceBetweenPoints = 0.75f;
                }
                break;
            case CollectiblePlacerPreset.TifaJump: {
                    placer.start.point = new Vector3(-2.25f, 2.2f, 0.0f);
                    placer.startTangent.point = new Vector3(-2.0f, 5.5f, 0.0f);
                    placer.end.point = new Vector3(1.0f, 5.0f, 0.0f);
                    placer.endTangent.point = new Vector3(0.0f, 8.0f, 0.0f);
                    placer.spaceBetweenPoints = 0.75f;
                }
                break;
            case CollectiblePlacerPreset.HopDown: {
                    placer.start.point = new Vector3(-0.35f, 1.0f, 0.0f);
                    placer.startTangent.point = new Vector3(-0.2f, 1.0f, 0.0f);
                    placer.end.point = new Vector3(1.3f, -0.5f, 0.0f);
                    placer.endTangent.point = new Vector3(1.2f, 2.0f, 0.0f);
                    placer.spaceBetweenPoints = 0.75f;
                }
                break;
            case CollectiblePlacerPreset.LeapDown: {
                    placer.start.point = new Vector3(-0.5f, 2.0f, 0.0f);
                    placer.startTangent.point = new Vector3(0.6f, 4.5f, 0.0f);
                    placer.end.point = new Vector3(5.5f, -5.0f, 0.0f);
                    placer.endTangent.point = new Vector3(4.0f, 4.5f, 0.0f);
                    placer.spaceBetweenPoints = 0.75f;
                }
                break;

                //case CollectiblePlacerPreset.TIFAJUMP: {
                //        placer.start.point = new Vector3(0.0f, 0.0f, 0.0f);
                //        placer.startTangent.point = new Vector3(0.0f, 0.0f, 0.0f);
                //        placer.end.point = new Vector3(0.0f, 0.0f, 0.0f);
                //        placer.endTangent.point = new Vector3(0.0f, 0.0f, 0.0f);
                //        placer.spaceBetweenPoints = 0.75f;
                //    }
                //    break;
        }
    }
}
