using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of all player variables and components
public class Player : MonoBehaviour {
    public PlayerMovementAdventure movement;
    void Start() {
        movement = GetComponent<PlayerMovementAdventure>();
    }
}
