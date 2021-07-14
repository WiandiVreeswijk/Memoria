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
        public Material skybox;
        public bool fog;

        public SceneDefinition() {
            name = "";
            scene = null;
            skybox = null;
            fog = false;
        }
    }

    private bool wijkSceneLoaded = false;
    private GameObject[] wijkParentObjects;

    public static List<string> loadedScenes = new List<string>();
    public SceneDefinition wijkScene;
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

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Wijk") {
            wijkParentObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            wijkSceneLoaded = true;
            activeScene = wijkScene;
        }

        if (activeScene == null) {
            Debug.LogError($"Current scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} not found in scene manager!");
        }
    }

    private void SetWijkSceneActive(bool active) {
        foreach (var obj in wijkParentObjects) obj.SetActive(active);
    }

    public bool IsLoadingScene() {
        return isLoadingScene;
    }

    public void SetScene(string sceneName) {
        if (sceneDefinitionsMap.TryGetValue(sceneName, out SceneDefinition scene)) {
            if (isLoadingScene || scene == activeScene) return;
            Globals.MenuController.BlackScreenFadeIn(2.0f).OnComplete(() => StartCoroutine(LoadScene(scene)));
        } else Debug.LogError($"[SceneManager] scene has not been registered {sceneName}");
    }

    public void SetScene(SceneDefinition scene) {
        if (isLoadingScene || scene == activeScene) return;
        Globals.MenuController.BlackScreenFadeIn(2.0f).OnComplete(() => StartCoroutine(LoadScene(scene)));
    }

    //#Todo Teleport back to previous position in wijk scene?
    //#Todo Fix audio not stopping on scene switch. 
    //DOTween.To(() => cam.m_Lens.FieldOfView, x => cam.m_Lens.FieldOfView = x, 120, 1.0f).SetEase(Ease.InExpo);
    //#Todo Global camera FOV change. 
    private IEnumerator LoadScene(SceneDefinition scene) {
        isLoadingScene = true;

        AsyncOperation unloadOperation = null;
        AsyncOperation loadOperation = null;
        SceneDefinition oldScene = activeScene;
        activeScene = scene;
        if (oldScene == wijkScene) { //Leaving wijk scene
            SetWijkSceneActive(false);
            loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.scene, LoadSceneMode.Additive);
        } else { //Entering wijk scene
            if (wijkSceneLoaded) { //Wijk scene has been loaded
                SetWijkSceneActive(true);
                unloadOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(oldScene.scene);
                Globals.Initialize(Globals.GlobalsType.NEIGHBORHOOD);
            } else { //Wijk scene has not been loaded before
                loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.scene, LoadSceneMode.Additive);
                unloadOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(oldScene.scene);
            }
        }
        if (loadOperation != null) loadOperation.allowSceneActivation = false; //Don't activate new scene

        RenderSettings.skybox = scene.skybox; //Fix unity bugs
        RenderSettings.fog = scene.fog;


        if (loadOperation != null) { //Wait for load
            while (loadOperation.progress < 0.9f) yield return null;
            loadOperation.allowSceneActivation = true;
        }
        if (unloadOperation != null)
            while (unloadOperation.progress < 0.9f) {
                print(unloadOperation.progress);
                yield return null;
            }

        yield return new WaitForSeconds(1.0f); //Prevent stutter
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

    public List<SceneDefinition> GetAllLevelScenes() {
        return levelSceneDefinitions;
    }

    public SceneDefinition GetActiveScene() {
        return activeScene;
    }

    public bool IsInWijkScene() {
        return activeScene == wijkScene;
    }

    public void SetWijkScene() {
        SetScene(wijkScene);
    }
}
