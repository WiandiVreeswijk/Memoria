using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(CloudSpawner), true)]
public class CloudSpawnerEditor : Editor {
    BoxBoundsHandle boxBounds;

    void OnEnable() {
        boxBounds = new BoxBoundsHandle();
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnDisable() {
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    private void OnUndoRedo() {
        CloudSpawner cSpawner = (CloudSpawner)target;
        cSpawner.Reset();
    }

    public void OnSceneGUI() {
        CloudSpawner cSpawner = (CloudSpawner)target;
        boxBounds.center = cSpawner.spawnArea.center + cSpawner.transform.position;
        boxBounds.size = cSpawner.spawnArea.size;

        EditorGUI.BeginChangeCheck();
        boxBounds.DrawHandle();
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(cSpawner, "cSpawner Bounds");
            cSpawner.spawnArea.center = boxBounds.center - cSpawner.transform.position;
            cSpawner.spawnArea.size = boxBounds.size;
            cSpawner.Reset();
            Debug.Log("CloudSpawnerSetDirty");
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnInspectorGUI() {
        CloudSpawner cSpawner = (CloudSpawner)target;

        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck()) {
            cSpawner.Reset();
        }

        bool isRunningInEditor = cSpawner.IsRunningInEditor();
        GUI.backgroundColor = isRunningInEditor ? Color.red : Color.green;
        if (GUILayout.Button(isRunningInEditor ? "Disable run in editor" : "Enable run in editor")) {
            cSpawner.Toggle();
        }

    }
}


