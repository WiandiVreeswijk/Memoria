using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneOpener : EditorWindow {
    [MenuItem("Scene/Wijk")]
    public static void Wijk() {
        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Wijk.unity");
    }

    [MenuItem("Scene/Chasing")]
    public static void Chasing() {
        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/ChasingLevel/ChasingLevelScene.unity");
    }

    [MenuItem("Scene/UI")]
    public static void UI() {
        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/UI.unity");
    }

    [MenuItem("Scene/Persistent")]
    public static void Persistent() {
        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Persistent.unity");
    }
}