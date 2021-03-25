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
    public UnityEngine.UI.Image image;

    void Start() {

    }


    public void EnterMemory() {
        StartCoroutine(EnterMemoryCoroutine());
    }

    IEnumerator EnterMemoryCoroutine() {
        bool finishedTransition = false;
        DOTween.To(() => cam.m_Lens.FieldOfView, x => cam.m_Lens.FieldOfView = x, 120, 1.0f).SetEase(Ease.InExpo);
        DOTween.To(() => image.color, x => image.color = x, Color.black, 1.0f).SetEase(Ease.InExpo).OnComplete(() => finishedTransition = true);
        var asyncTask = SceneManager.LoadSceneAsync(memorySceneName);
        asyncTask.allowSceneActivation = false;
        while (!finishedTransition || asyncTask.progress < 0.9f) {
            yield return null;
        }
        asyncTask.allowSceneActivation = true;
    }

    public void LeaveMemory() {

    }
}
