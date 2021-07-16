using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerActivatorEvent : MonoBehaviour, IEnterActivatable, IExitActivatable {
    public TriggerActivatorType type;
    public UnityEvent triggerEvent;

    public void ActivateEnter() {
        if (type == TriggerActivatorType.ENTER) {
            triggerEvent.Invoke();
        }
    }

    public void ActivateExit() {
        if (type == TriggerActivatorType.EXIT) {
            triggerEvent.Invoke();
        }
    }
}
