using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PersistenceManager : MonoBehaviour {

    public CinemachineVirtualCamera cam;
    public string wijkSceneName;
    public string memorySceneName;

    void Start() {

    }


    public void EnterMemory() {
        StartCoroutine(LoadScene(memorySceneName));
    }

    IEnumerator LoadScene(string name) {
        bool finishedTransition = false;
        DOTween.To(() => cam.m_Lens.FieldOfView, x => cam.m_Lens.FieldOfView = x, 120, 1.0f).SetEase(Ease.InExpo);
        Globals.MenuController.BlackScreenFadeIn(1.0f);
        var asyncTask = SceneManager.LoadSceneAsync(name);
        asyncTask.allowSceneActivation = false;
        while (!finishedTransition || asyncTask.progress < 0.9f) {
            yield return null;
        }
        asyncTask.allowSceneActivation = true;
    }

    public void LeaveMemory() {
        StartCoroutine(LoadScene(wijkSceneName));
    }
}
