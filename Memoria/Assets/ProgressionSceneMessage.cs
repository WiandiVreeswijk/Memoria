using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSceneMessage : ProgressionNodeComponent {
    public string OnEnterNodeMessage = "";
    public string OnExitNodeMessage = "";

    public override void OnEnterNode() {
        Debug.Log(OnEnterNodeMessage);
    }

    public override void OnExitNode() {
        Debug.Log(OnExitNodeMessage);
    }

    public override string GetName() => "Message";
}
