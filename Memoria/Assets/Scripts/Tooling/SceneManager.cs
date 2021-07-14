using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

//Utility class for keeping track of our scenes and switching between them
public class SceneManager : MonoBehaviour {
    [Serializable]
    public class SceneDefinition {
        public string name;
        public SceneReference scene;

        public SceneDefinition() {
            name = "";
            scene = null;
        }
    }

    public List<SceneDefinition> sceneDefinitions = new List<SceneDefinition>();
    public Dictionary<string, SceneDefinition> sceneDefinitionsMap = new Dictionary<string, SceneDefinition>();
    private bool isLoadingScene = false;

    private void Start() {
        for (int i = 0; i < sceneDefinitions.Count; i++) {
            if (sceneDefinitions[i].name.Length == 0) continue;
            if (sceneDefinitionsMap.ContainsKey(sceneDefinitions[i].name))
                Debug.LogError($"[SceneManager] duplicate scene definition registered under {sceneDefinitions[i].name}");
            else sceneDefinitionsMap.Add(sceneDefinitions[i].name, sceneDefinitions[i]);
        }
    }

    public bool IsLoadingScene() {
        return isLoadingScene;
    }

    public void SetScene(string sceneName) {
        if (sceneDefinitionsMap.TryGetValue(sceneName, out SceneDefinition scene)) {
            if (isLoadingScene) return;
            StartCoroutine(LoadScene(scene));
        } else Debug.LogError($"[SceneManager] scene has not been registered {sceneName}");
    }

    //#Todo Teleport back to previous position in wijk scene?
    //#Todo Fix audio not stopping on scene switch. 
    //#Todo Global camera FOV change. 
    private IEnumerator LoadScene(SceneDefinition scene) {
        isLoadingScene = true;
        bool finishedTransition = false;
        //DOTween.To(() => cam.m_Lens.FieldOfView, x => cam.m_Lens.FieldOfView = x, 120, 1.0f).SetEase(Ease.InExpo);
        Globals.MenuController.BlackScreenFadeIn(2.0f).OnComplete(() => finishedTransition = true);
        var asyncTask = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.scene, LoadSceneMode.Single);
        asyncTask.allowSceneActivation = false;
        while (!finishedTransition || asyncTask.progress < 0.9f) {
            yield return null;
        }
        isLoadingScene = false;
        asyncTask.allowSceneActivation = true;
    }

    public static void LoadSceneIfNotActive(string sceneName) {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++) {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name == sceneName && scene.isLoaded) return;
        }

        print("Loaded scene " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    //public void LoadUI() {
    //    StartCoroutine(LoadScene());
    //}
    //
    //private IEnumerator LoadScene() {
    //    AsyncOperation asyncLoadLevel = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    //    while (!asyncLoadLevel.isDone) yield return null;
    //    yield return new WaitForEndOfFrame();
    //    Globals.InitializeUI();
    //}
}
