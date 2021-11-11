using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionNode {
    public string name;
    Dictionary<string, ProgressionNode> paths = new Dictionary<string, ProgressionNode>();

    public ProgressionNode(string name) {
        this.name = name;
    }

    public ProgressionNode GetPath(string path) {
        paths.TryGetValue(path, out ProgressionNode node);
        return node;
    }

    public void AddPathNode(string path, ProgressionNode node) {
        paths.Add(path, node);
    }

    public Dictionary<string, ProgressionNode> GetPaths() {
        return paths;
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
