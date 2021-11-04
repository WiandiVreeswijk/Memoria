using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Progression data")]
[System.Serializable]
public class ProgressionData : ScriptableObject {
    [HideInInspector] public List<Node> nodes;
    [HideInInspector] public List<Connection> connections;
}
