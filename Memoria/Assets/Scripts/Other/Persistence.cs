using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//#Todo Do we even still need this class? Persistent data storage perhaps?
public class Persistence : MonoBehaviour {
    //public CinemachineVirtualCamera cam;
    //public string wijkSceneName;
    //public string memorySceneName;

    public void EnterMemory() {
        //StartCoroutine(LoadScene(memorySceneName));
    }

    //#Todo: How are we gonna handle camera FOV changes on a global scale?
    //#Todo: Camera FOV change
    

    public void LeaveMemory() {
        //StartCoroutine(LoadScene(wijkSceneName));
    }
}
