using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {
    public static ProgressionManager _Instance;
    public ProgressionData progressionData;
    private ProgressionTree progressionTree;
    private ProgressionNode activeNode;

    void Awake() {
        if (_Instance != null) {
            Destroy(gameObject);
            throw new System.Exception("Multiple instances of progression manager found");
        }
        _Instance = this;

        BuildTree();
    }

    private void BuildTree() {
        if (progressionData == null) throw new System.Exception("No progression data found in progression manager");
        progressionTree = ProgressionTree.CreateFromData(progressionData);
        activeNode = progressionTree.baseNode;
    }

    private void _Progress(string progressTag) {
        ProgressionNode newNode = activeNode.GetPath(progressTag);
        if (newNode != null) {
            activeNode.OnProgressionExit();
            activeNode = newNode;
            activeNode.OnProgressionEnter();
        } else {
            Debug.LogError($"Progression {progressTag} not found in node {activeNode.name}");
        }
    }

    public static void Progress(string progressTag) {
        _Instance._Progress(progressTag);
    }

    public ProgressionNode GetActiveNode() {
        return activeNode;
    }
}
