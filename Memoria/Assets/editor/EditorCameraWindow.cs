using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorCameraWindow : EditorWindow {
    [MenuItem("WAEM/Editor camera")]
    public static void ShowWindow() {
        GetWindow<EditorCameraWindow>("Editor camera");
    }


    private void OnGUI() {
        //To replace all empty materials in the scene
        //public Material mat;
        //mat = EditorGUILayout.ObjectField(mat, typeof(Material), false) as Material; ;
        //
        //if (GUILayout.Button("A")) {
        //    MeshRenderer[] objs = GameObject.FindObjectsOfType<MeshRenderer>();
        //    foreach (var obj in objs) {
        //        bool nu = false;
        //        for (int i = 0; i < obj.sharedMaterials.Length; i++) {
        //            if (obj.sharedMaterials[i] == null) {
        //                nu = true;
        //                continue;
        //            }
        //
        //        }
        //
        //        if (nu) {
        //            Debug.Log("Null");
        //            Material[] mats = new Material[obj.sharedMaterials.Length];
        //            for (int i = 0; i < obj.sharedMaterials.Length; i++) mats[i] = mat;
        //            obj.sharedMaterials = mats;
        //        }
        //    }
        //}
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
