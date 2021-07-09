using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class WAEMTestingTab : IWAEMTab {
    //thirdpersonCamera
    GameObject thirdPersonCameraObject;
    CinemachineFreeLook thirdPersonCamera;
    int fieldOfView;
    bool showPositionCamSpeed = true;
    float camSpeedX, camSpeedY;
    bool showPositionCamAccelDecel = true;
    float camAccelX, camDecelX, camAccelY, camDecelY;

    public void Initialize() {

    }

    public void OnGUI(EditorWindow window, GUIStyle style) {
        bool enabled = IsInstantPlayEnabled();
        EditorGUILayout.LabelField($"<b>InstantPlay</b> <b><color={(enabled ? "green" : "red")}>{(enabled ? "enabled" : "disabled")}</color></b>", style);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Toggle between full game and instant play");
        if (GUILayout.Button("Toggle instant play", GUILayout.Height(40))) {
            ToggleInstantPlay();
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Camera options", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        ThirdPersonCamera();
        ThirdPersonMovement();
        EditorGUI.indentLevel--;
    }

    public void OnUpdate() {

    }

    public void OnSelectionChange(EditorWindow window) {

    }

    public void OnDestroy() {

    }

    private void ThirdPersonCamera() {
        //finds the cinemachine freelook camera from the given object
        EditorGUILayout.LabelField("Third person camera options", EditorStyles.boldLabel);

        thirdPersonCameraObject = EditorGUILayout.ObjectField("tp camera object", thirdPersonCameraObject, typeof(GameObject), true) as GameObject;
        if (thirdPersonCameraObject != null) {
            thirdPersonCamera = thirdPersonCameraObject.GetComponent<CinemachineFreeLook>();
            Debug.Log("TestingTab");
            EditorUtility.SetDirty(thirdPersonCameraObject);

            EditorGUILayout.LabelField("Third person camera options", EditorStyles.boldLabel);

            //slider to adjust field of view
            fieldOfView = EditorGUILayout.IntSlider("Field of view", fieldOfView, 40, 150);
            thirdPersonCamera.m_Lens.FieldOfView = fieldOfView;

            //slider to adjust sensitivity
            showPositionCamSpeed = EditorGUILayout.Foldout(showPositionCamSpeed, "Camera Speed");
            if (showPositionCamSpeed)
                if (Selection.activeTransform) {
                    camSpeedX = EditorGUILayout.Slider("cam speed X", camSpeedX, 300, 1000);
                    camSpeedY = EditorGUILayout.Slider("cam speed Y", camSpeedY, 3, 10);
                    thirdPersonCamera.m_XAxis.m_MaxSpeed = camSpeedX;
                    thirdPersonCamera.m_YAxis.m_MaxSpeed = camSpeedY;
                }

            if (!Selection.activeTransform) {
                showPositionCamSpeed = false;
            }

            //slider to adjust camera accelaration and decelaration
            showPositionCamAccelDecel = EditorGUILayout.Foldout(showPositionCamAccelDecel, "Accel & Decel");
            if (showPositionCamAccelDecel)
                if (Selection.activeTransform) {
                    camAccelX = EditorGUILayout.Slider("cam accel X", camAccelX, 0, 1);
                    camDecelX = EditorGUILayout.Slider("cam decel X", camDecelX, 0, 1);
                    camAccelY = EditorGUILayout.Slider("cam accel Y", camAccelY, 0, 1);
                    camDecelY = EditorGUILayout.Slider("cam decel Y", camDecelY, 0, 1);
                    thirdPersonCamera.m_XAxis.m_AccelTime = camAccelX;
                    thirdPersonCamera.m_XAxis.m_DecelTime = camDecelX;
                    thirdPersonCamera.m_YAxis.m_AccelTime = camAccelY;
                    thirdPersonCamera.m_YAxis.m_DecelTime = camDecelY;
                }

            if (!Selection.activeTransform) {
                showPositionCamAccelDecel = false;
            }
        } else {
            EditorGUILayout.HelpBox("Third person camera object is missing", MessageType.Warning);
        }
    }

    private void ThirdPersonMovement() {
        EditorGUILayout.LabelField("Third person movement options", EditorStyles.boldLabel);
    }

    private bool IsInstantPlayEnabled() {
        GameObject instantPlayObjects = GameObject.Find("InstantPlayObjects");
        GameObject obj = GameObject.Find("InstantPlayOn");
        return obj != null;
    }
    private void ToggleInstantPlay() {
        GameObject instantPlayObjects = GameObject.Find("InstantPlayObjects");
        GameObject obj = GameObject.Find("InstantPlayOn");
        bool instantPlayOn = obj != null;
        if (!instantPlayOn) {
            new GameObject("InstantPlayOn").transform.parent = instantPlayObjects.transform;
            instantPlayOn = true;
        } else {
            Object.DestroyImmediate(obj);
            instantPlayOn = false;
        }

        instantPlayObjects.transform.GetChild(0).gameObject.SetActive(!instantPlayOn);
    }
}
