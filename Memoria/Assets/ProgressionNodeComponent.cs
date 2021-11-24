using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProgressionNodeComponent : MonoBehaviour {
    public abstract void OnEnterNode();
    public abstract void OnExitNode();
    public abstract string GetName();
}