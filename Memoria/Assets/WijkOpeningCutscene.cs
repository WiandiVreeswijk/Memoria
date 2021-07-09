using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class WijkOpeningCutscene : MonoBehaviour {
    public CinemachineVirtualCamera mainMenuCamera;
    public PlayableDirector playableDirector;

    public void Trigger() {
        //cursorLocker.LockMouse();
        mainMenuCamera.Priority = 10;
        playableDirector.time = 0;
        playableDirector.Evaluate();
        playableDirector.Play();
    }
}
