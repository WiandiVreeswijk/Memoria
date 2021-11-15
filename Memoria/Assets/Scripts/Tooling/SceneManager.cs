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
    public List<SceneDefinition> keySceneDefinitions = new List<SceneDefinition>();
    public List<SceneDefinition> levelSceneDefinitions = new List<SceneDefinition>();
    public Dictionary<string, SceneDefinition> sceneDefinitionsMap = new Dictionary<string, SceneDefinition>();
    private SceneDefinition activeScene;
    private bool isLoadingScene = false;

    private void Awake() {
        for (int i = 0; i < levelSceneDefinitions.Count; i++) {
            if (levelSceneDefinitions[i].scene.ScenePath.Contains(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ".unity")) activeScene = levelSceneDefinitions[i];
            if (levelSceneDefinitions[i].name.Length == 0) continue;
            if (sceneDefinitionsMap.ContainsKey(levelSceneDefinitions[i].name))
                Debug.LogError($"[SceneManager] duplicate scene definition registered under {levelSceneDefinitions[i].name}");
            else sceneDefinitionsMap.Add(levelSceneDefinitions[i].name, levelSceneDefinitions[i]);
        }

        if (activeScene == null) {
            Debug.LogError($"Current scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} not found in scene manager!");
        }
    }

    public bool IsLoadingScene() {
        return isLoadingScene;
    }

    public void SetScene(string sceneName) {
        Debug.Log(sceneName);
        if (sceneDefinitionsMap.TryGetValue(sceneName, out SceneDefinition scene)) {
            if (isLoadingScene || scene == activeScene) return;
            Globals.CinemachineManager.SetInputEnabled(false);
            Globals.MenuController.BlackScreenFadeIn(2.0f, true).OnComplete(() => StartCoroutine(LoadScene(scene))).SetUpdate(true);
        } else Debug.LogError($"[SceneManager] scene has not been registered {sceneName}");
    }

    public void SetScene(SceneDefinition scene) {
        if (isLoadingScene || scene == activeScene) return;
        Globals.MenuController.BlackScreenFadeIn(2.0f, true).OnComplete(() => StartCoroutine(LoadScene(scene)));
    }

    //#Todo Teleport back to previous position in wijk scene?
    //#Todo Fix audio not stopping on scene switch. 
    //DOTween.To(() => cam.m_Lens.FieldOfView, x => cam.m_Lens.FieldOfView = x, 120, 1.0f).SetEase(Ease.InExpo);
    //#Todo Global camera FOV change. 
    private IEnumerator LoadScene(SceneDefinition scene) {
        string oldSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        isLoadingScene = true;
        activeScene = scene;
        AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.scene, LoadSceneMode.Single);
        loadOperation.allowSceneActivation = false; //Don't activate new scene
        while (loadOperation.progress < 0.9f) yield return null;
        loadOperation.allowSceneActivation = true;

        //yield return new WaitForSeconds(1.0f); //Prevent stutter
        while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == oldSceneName) yield return null;
        isLoadingScene = false;
        Globals.CursorManager.LockMouse();
        print("Active scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Globals.MenuController.BlackScreenFadeOut(2.0f).OnComplete(() => Globals.CinemachineManager.SetInputEnabled(true)).SetUpdate(true);
    }

    public static void LoadSceneIfNotActive(string sceneName) {
        if (loadedScenes.Contains(sceneName)) return;
        loadedScenes.Add(sceneName);
        print("Loaded scene " + sceneName + " + " + loadedScenes.Count);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public List<SceneDefinition> GetAllLevelScenes() {
        return levelSceneDefinitions;
    }

    public SceneDefinition GetActiveScene() {
        return activeScene;
    }
}
