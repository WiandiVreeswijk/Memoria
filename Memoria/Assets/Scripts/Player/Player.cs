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

    private Elena25DCameraController cameraController;
    public Elena25DCameraController CameraController => cameraController;

    private PlayerDeath playerDeath;
    public PlayerDeath PlayerDeath => playerDeath;

    void Start() {
        cameraController = GetComponent<Elena25DCameraController>();
        playerMovement25D = GetComponent<PlayerMovement25D>();
        visualEffects = GetComponent<PlayerVisualEffects>();
        playerMovementAdventure = GetComponent<PlayerMovementAdventure>();
        playerDeath = GetComponent<PlayerDeath>();
    }
}
