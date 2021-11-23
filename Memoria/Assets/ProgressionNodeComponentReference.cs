using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ProgressionNodeComponentReference {
    [NonSerialized] public ProgressionNodeComponent component;
    [NonSerialized] public string errorMessage = "";
    [SerializeField] public string scenePath = "";
    [SerializeField] public string ID = "";

    public ProgressionNodeComponentReference() { }

    public ProgressionNodeComponentReference(string scenePath, string ID) {
        this.scenePath = scenePath;
        this.ID = ID;
    }

    public void Initialize() {
        if (scenePath.Length > 0) {
            if (IsSceneOpen(scenePath)) {
                if (GlobalObjectId.TryParse(ID, out GlobalObjectId goi)) {
                    component = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(goi) as ProgressionNodeComponent;
                    if (component != null) {
                        errorMessage = "";
                        return;
                    }
                }

                errorMessage = "Component not found in scene";
                //TODO: reset data if node not found?
            } else {
                errorMessage = $"This component is located a different scene:\n{Path.GetFileName(scenePath)}";
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

    public void OnEnterNode() {
        if (component == null) Initialize();
        if (component != null) component.OnEnterNode();
    }

    public void OnExitNode() {
        if (component == null) Initialize();
        if (component != null) component.OnExitNode();
    }

    public bool IsEmpty() {
        return component == null && ID.Length == 0 && scenePath.Length == 0;
    }
}
