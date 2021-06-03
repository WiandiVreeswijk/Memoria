using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of all player variables and components
public class Player : MonoBehaviour {
    private PlayerMovement25D playerMovement25D;
    public PlayerMovement25D PlayerMovement25D { get { return playerMovement25D; } }

    private PlayerMovementAdventure playerMovementAdventure;
    public PlayerMovementAdventure PlayerMovementAdventure { get { return playerMovementAdventure; } }

    private PlayerVisualEffects visualEffects;
    public PlayerVisualEffects VisualEffects { get { return visualEffects; } }

    void Start() {
        playerMovementAdventure = GetComponent<PlayerMovementAdventure>();
        playerMovement25D = GetComponent<PlayerMovement25D>();
        visualEffects = GetComponent<PlayerVisualEffects>();
    }
}
