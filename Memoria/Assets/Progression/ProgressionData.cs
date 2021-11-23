using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenuAttribute]
public class ProgressionData : ScriptableObject {
    public NodeData[] nodeDataCollection = new NodeData[0];

    public int baseNodeID;
    public int nodeID;

    [Serializable]
    public class NodeConnection {
        public string name;
        public int reference;
    }


    [Serializable]
    public class NodeData {
        public int id;
        public string name;
        public Vector2 position;
        public NodeConnection[] connections;

        [SerializeReference] public ProgressionNodeComponentReference[] sceneNodes;
    }
}
