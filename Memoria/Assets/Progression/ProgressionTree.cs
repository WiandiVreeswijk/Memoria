using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTree {
    public RuntimeNode baseNode;

    public static ProgressionTree CreateFromData(ProgressionData progressionData) {
        ProgressionTree tree = new ProgressionTree();
        Dictionary<int, RuntimeNode> nodesDict = new Dictionary<int, RuntimeNode>();
        //Create nodes
        for (int i = 0; i < progressionData.nodeDataCollection.Length; i++) {
            ProgressionData.NodeData data = progressionData.nodeDataCollection[i];
            RuntimeNode node = new RuntimeNode(data.name, data.sceneNodes);
            if (progressionData.baseNodeID == data.id) tree.baseNode = node;
            nodesDict.Add(data.id, node);
        }

        //Create connections
        for (int i = 0; i < progressionData.nodeDataCollection.Length; i++) {
            ProgressionData.NodeData data = progressionData.nodeDataCollection[i];
            RuntimeNode node = nodesDict[data.id];
            for (int j = 0; j < data.connections.Length; j++) {
                ProgressionData.NodeConnection connection = data.connections[j];
                if (connection.reference == -1) continue;
                node.AddPathNode(data.connections[j].name, nodesDict[connection.reference]);
            }
        }
        return tree;
    }
}
