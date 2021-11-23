using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeNode {
    public string name;
    Dictionary<string, RuntimeNode> paths = new Dictionary<string, RuntimeNode>();
    ProgressionNodeComponentReference[] sceneNodeReferences;

    public RuntimeNode(string name, ProgressionNodeComponentReference[] sceneNodeReferences) {
        this.name = name;
        this.sceneNodeReferences = sceneNodeReferences;
    }

    public RuntimeNode GetPath(string path) {
        paths.TryGetValue(path, out RuntimeNode node);
        return node;
    }

    public void AddPathNode(string path, RuntimeNode node) {
        paths.Add(path, node);
    }

    public Dictionary<string, RuntimeNode> GetPaths() {
        return paths;
    }

    public void OnEnterNode() {
        foreach (var progressionSceneNodeReference in sceneNodeReferences) {
            progressionSceneNodeReference.OnEnterNode();
        }
    }

    public void OnExitNode() {
        foreach (var progressionSceneNodeReference in sceneNodeReferences) {
            progressionSceneNodeReference.OnExitNode();
        }
    }

    public void Print() {
        string str = "";
        str += name;
        foreach (var node in paths.Values) {
            str += "\n" + node.name;
        }

        Debug.Log(str);

        foreach (var node in paths.Values) {
            node.Print();
        }
    }
}
