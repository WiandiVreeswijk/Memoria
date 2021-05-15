using UnityEditor;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : EditorWindow
{
    GameObject thirdPersonCameraObject;
    CinemachineFreeLook thirdPersonCamera;

    int fieldOfView;

    bool showPositionCamSpeed = true;
    float camSpeedX, camSpeedY;

    bool showPositionCamAccelDecel = true;
    float camAccelX, camDecelX, camAccelY, camDecelY;


    [MenuItem("WAEM/Camera tooling")]
    public static void ShowWindow()
    {
        var window = GetWindow<CameraBehaviour>("WAEM tooling");
        //window.Initialize();
    }

    private void OnGUI()
    {
        //finds the cinemachine freelook camera from the given object
        thirdPersonCameraObject = EditorGUILayout.ObjectField("tp camera object", thirdPersonCameraObject, typeof(GameObject), true) as GameObject;
        if (thirdPersonCameraObject != null)
        {
            thirdPersonCamera = thirdPersonCameraObject.GetComponent<CinemachineFreeLook>();
            EditorUtility.SetDirty(thirdPersonCameraObject);

            GUILayout.Label("Third person camera options", EditorStyles.boldLabel);

            //slider to adjust field of view
            fieldOfView = EditorGUILayout.IntSlider("Field of view", fieldOfView, 40, 150);
            thirdPersonCamera.m_Lens.FieldOfView = fieldOfView;

            //slider to adjust sensitivity
            showPositionCamSpeed = EditorGUILayout.Foldout(showPositionCamSpeed, "Camera Speed");
            if (showPositionCamSpeed)
                if (Selection.activeTransform)
                {
                    camSpeedX = EditorGUILayout.Slider("cam speed X", camSpeedX, 300, 1000);
                    camSpeedY = EditorGUILayout.Slider("cam speed Y", camSpeedY, 3, 10);
                    thirdPersonCamera.m_XAxis.m_MaxSpeed = camSpeedX;
                    thirdPersonCamera.m_YAxis.m_MaxSpeed = camSpeedY;
                }

            if (!Selection.activeTransform)
            {
                showPositionCamSpeed = false;
            }

            //slider to adjust camera accelaration and decelaration
            showPositionCamAccelDecel = EditorGUILayout.Foldout(showPositionCamAccelDecel, "Accel & Decel");
            if (showPositionCamAccelDecel)
                if (Selection.activeTransform)
                {
                    camAccelX = EditorGUILayout.Slider("cam accel X", camAccelX, 0, 1);
                    camDecelX = EditorGUILayout.Slider("cam decel X", camDecelX, 0, 1);
                    camAccelY = EditorGUILayout.Slider("cam accel Y", camAccelY, 0, 1);
                    camDecelY = EditorGUILayout.Slider("cam decel Y", camDecelY, 0, 1);
                    thirdPersonCamera.m_XAxis.m_AccelTime = camAccelX;
                    thirdPersonCamera.m_XAxis.m_DecelTime = camDecelX;
                    thirdPersonCamera.m_YAxis.m_AccelTime = camAccelY;
                    thirdPersonCamera.m_YAxis.m_DecelTime = camDecelY;
                }

            if (!Selection.activeTransform)
            {
                showPositionCamAccelDecel = false;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Third person camera object is missing", MessageType.Warning);
        }
    }

}
