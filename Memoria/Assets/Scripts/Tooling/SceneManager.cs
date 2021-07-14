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

    public static List<string> loadedScenes = new List<string>();
    public List<SceneDefinition> sceneDefinitions = new List<SceneDefinition>();
    public Dictionary<string, SceneDefinition> sceneDefinitionsMap = new Dictionary<string, SceneDefinition>();
    private SceneDefinition activeScene;
    private bool isLoadingScene = false;

    private void Awake() {
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoaded;
        //UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SceneUnloaded;

        for (int i = 0; i < sceneDefinitions.Count; i++) {
            if (sceneDefinitions[i].scene.ScenePath
                .Contains(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ".unity"))
                activeScene = sceneDefinitions[i];
            if (sceneDefinitions[i].name.Length == 0) continue;
            if (sceneDefinitionsMap.ContainsKey(sceneDefinitions[i].name))
                Debug.LogError($"[SceneManager] duplicate scene definition registered under {sceneDefinitions[i].name}");
            else sceneDefinitionsMap.Add(sceneDefinitions[i].name, sceneDefinitions[i]);
        }
        if (activeScene == null) {
            Debug.LogError($"Current scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} not found in scene manager!");
        }
    }

    //private void SceneLoaded(Scene scene, LoadSceneMode mode) {
    //    loadedScenes.Add(scene.name);
    //}
    //
    //private void SceneUnloaded(Scene scene) {
    //    loadedScenes.Remove(scene.name);
    //}

    public bool IsLoadingScene() {
        return isLoadingScene;
    }

    public void SetScene(string sceneName) {
        if (sceneDefinitionsMap.TryGetValue(sceneName, out SceneDefinition scene)) {
            if (isLoadingScene) return;
            Globals.MenuController.BlackScreenFadeIn(2.0f).OnComplete(() => StartCoroutine(LoadScene(scene)));

            ;
        } else Debug.LogError($"[SceneManager] scene has not been registered {sceneName}");
    }

    //#Todo Teleport back to previous position in wijk scene?
    //#Todo Fix audio not stopping on scene switch. 
    //#Todo Global camera FOV change. 
    private IEnumerator LoadScene(SceneDefinition scene) {
        isLoadingScene = true;
        //DOTween.To(() => cam.m_Lens.FieldOfView, x => cam.m_Lens.FieldOfView = x, 120, 1.0f).SetEase(Ease.InExpo);
        var asyncTask = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.scene, LoadSceneMode.Additive);
        asyncTask.allowSceneActivation = false;
        foreach (var a in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) {
            a.SetActive(false);
        }
        while (asyncTask.progress < 0.9f) yield return null;
        asyncTask.allowSceneActivation = true;

        activeScene = scene;
        yield return new WaitForSeconds(1.0f);
        isLoadingScene = false;
        Globals.MenuController.BlackScreenFadeOut(2.0f);
    }

    public static void LoadSceneIfNotActive(string sceneName) {
        if (loadedScenes.Contains(sceneName)) return;
        //for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++) {
        //    var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
        //    //print(scene.name + " + " + sceneName + " + " + loadedScenes.Contains(scene.name));
        //    if (scene.name == sceneName && loadedScenes.Contains(scene.name)) return;
        //}

        loadedScenes.Add(sceneName);
        print("Loaded scene " + sceneName + " + " + loadedScenes.Count);
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
    public List<SceneDefinition> GetAllScenes() {
        return sceneDefinitions;
    }

    public SceneDefinition GetActiveScene() {
        return activeScene;
    }
}
