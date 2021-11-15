using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ProgressionSceneNodeReference {
    [NonSerialized] public ProgressionSceneNode node;
    [NonSerialized] public string errorMessage = "";
    [SerializeField] public string scenePath = "";
    [SerializeField] public string ID;

    public ProgressionSceneNodeReference() { }

    public ProgressionSceneNodeReference(string scenePath, string ID) {
        this.scenePath = scenePath;
        this.ID = ID;
    }

    public void Initialize() {
        if (scenePath.Length > 0) {
            if (IsSceneOpen(scenePath)) {
                if (GlobalObjectId.TryParse(ID, out GlobalObjectId goi)) {
                    node = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(goi) as ProgressionSceneNode;
                    if (node != null) {
                        errorMessage = "";
                        return;
                    }
                }

                errorMessage = "Node not found in scene";
                //TODO: reset data if node not found?
            } else {
                errorMessage = $"This node is located a different scene:\n{Path.GetFileName(scenePath)}";
            }
        }
    }

    bool IsSceneOpen(string path) {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++) {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.path == path) return true;
        }

        return false;
    }

    public void OnEnterProgression() {
        if (node == null) Initialize();
        if (node != null) node.onEnterProgression.Invoke();
    }

    public void onExitProgression() {
        if (node == null) Initialize();
        if (node != null) node.onExitProgression.Invoke();
    }
}
