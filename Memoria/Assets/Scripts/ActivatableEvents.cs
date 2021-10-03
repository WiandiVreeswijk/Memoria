using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatableEvents : MonoBehaviour, IEnterActivatable, IExitActivatable {
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    public void ActivateEnter() {
        OnEnter.Invoke();
    }

    public void ActivateExit() {
        OnExit.Invoke();
    }
}
