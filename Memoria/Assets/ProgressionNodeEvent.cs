using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressionNodeEvent : ProgressionNodeComponent {
    public UnityEvent onEnterNode;
    public UnityEvent onExitNode;
    public override void OnEnterNode() {
        onEnterNode.Invoke();
    }

    public override void OnExitNode() {
        onExitNode.Invoke();
    }

    public override string GetName() => "Event";
}
