using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeNode {
    public string name;
    Dictionary<string, RuntimeNode> paths = new Dictionary<string, RuntimeNode>();
    ProgressionSceneNodeReference sceneNodeReference;

    public RuntimeNode(string name, ProgressionSceneNodeReference sceneNodeReference) {
        this.name = name;
        this.sceneNodeReference = sceneNodeReference;
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
        sceneNodeReference.OnEnterNode();
    }

    public void OnExitNode() {
        sceneNodeReference.OnExitNode();
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
