using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of all player variables and components
public class Player : MonoBehaviour {
    private PlayerMovement25D playerMovement25D;
    public PlayerMovement25D PlayerMovement25D => playerMovement25D;

    private PlayerMovementAdventure playerMovementAdventure;
    public PlayerMovementAdventure PlayerMovementAdventure => playerMovementAdventure;

    private PlayerVisualEffects visualEffects;
    public PlayerVisualEffects VisualEffects => visualEffects;

    private PlayerCameraController cameraController;
    public PlayerCameraController CameraController => cameraController;

    private PlayerDeath playerDeath;
    public PlayerDeath PlayerDeath => playerDeath;

    private Transform povPoint;
    public Vector3 HeadPosition => povPoint.position;

    private PlayerSound playerSound;
    public PlayerSound PlayerSound => playerSound;
    void Start() {
        cameraController = GetComponent<PlayerCameraController>();
        playerMovement25D = GetComponent<PlayerMovement25D>();
        visualEffects = GetComponent<PlayerVisualEffects>();
        playerMovementAdventure = GetComponent<PlayerMovementAdventure>();
        playerDeath = GetComponent<PlayerDeath>();
        povPoint = Utils.FindUniqueChildInTransform(transform, "POVPoint");
        playerSound = GetComponent<PlayerSound>();
    }
}
