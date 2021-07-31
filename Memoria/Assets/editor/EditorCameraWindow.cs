using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorCameraWindow : EditorWindow {
    [MenuItem("WAEM/Editor camera")]
    public static void ShowWindow() {
        GetWindow<EditorCameraWindow>("Editor camera");
    }

    private void OnGUI() {
        Camera camera = SceneView.lastActiveSceneView.camera;

        EditorGUILayout.LabelField("Set the position of the editor camera");
        EditorGUI.BeginChangeCheck();
        Vector3 pos = camera.transform.position;
        Vector3 rot = camera.transform.rotation.eulerAngles;
        pos.x = EditorGUILayout.FloatField("position x", pos.x);
        pos.y = EditorGUILayout.FloatField("position y", pos.y);
        pos.z = EditorGUILayout.FloatField("position z", pos.z);

        rot.x = EditorGUILayout.FloatField("rotation x", rot.x);
        rot.y = EditorGUILayout.FloatField("rotation y", rot.y);
        rot.z = EditorGUILayout.FloatField("rotation z", rot.z);

        if (EditorGUI.EndChangeCheck()) {
            var view = SceneView.lastActiveSceneView;
            if (view != null) {
                var target = new GameObject();
                target.transform.position = pos;
                target.transform.rotation = Quaternion.Euler(rot);
                view.AlignViewToObject(target.transform);
                GameObject.DestroyImmediate(target);
            }
        }
    }
}
