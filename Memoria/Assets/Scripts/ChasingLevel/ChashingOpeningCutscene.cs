using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChashingOpeningCutscene : MonoBehaviour {
    [Tooltip("Disable this to disable the opening cutscene.")]
    public bool isEnabled = true;
    void Start() {
        if (!isEnabled && Application.isEditor) {
            GetComponentInChildren<PlayableDirector>().enabled = false;
        }
    }
}
