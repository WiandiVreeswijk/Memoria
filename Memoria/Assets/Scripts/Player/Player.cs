using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of all player variables and components
public class Player : MonoBehaviour {
    //Global
    private Transform povPoint;
    public Vector3 HeadPosition => povPoint.position;

    private Transform eyePoint;
    public Vector3 EyePosition => eyePoint.position;

    //Wijk
    private PlayerMovementAdventure playerMovementAdventure;
    public PlayerMovementAdventure PlayerMovementAdventure => playerMovementAdventure;

    private PlayerVisualEffects visualEffects;
    public PlayerVisualEffects VisualEffects => visualEffects;

    //Levels
    private PlayerMovement25D playerMovement25D;
    public PlayerMovement25D PlayerMovement25D => playerMovement25D;

    private Player25DVisualEffects visualEffects25D;
    public Player25DVisualEffects VisualEffects25D => visualEffects25D;

    private PlayerCameraController cameraController;
    public PlayerCameraController CameraController => cameraController;

    private PlayerDeath playerDeath;
    public PlayerDeath PlayerDeath => playerDeath;

    private PlayerSound playerSound;
    public PlayerSound PlayerSound => playerSound;

    public void OnGlobalsInitialize() {
        playerMovementAdventure = GetComponent<PlayerMovementAdventure>();
        playerMovement25D = GetComponent<PlayerMovement25D>();
        cameraController = GetComponent<PlayerCameraController>();
        visualEffects25D = GetComponent<Player25DVisualEffects>();
        visualEffects = GetComponent<PlayerVisualEffects>();
        playerDeath = GetComponent<PlayerDeath>();
        playerSound = GetComponent<PlayerSound>();
        povPoint = Utils.FindUniqueChildInTransform(transform, "POVPoint");
    }

    public void OnGlobalsInitializeType(Globals.GlobalsType type) {
        switch (type) {
            case Globals.GlobalsType.DEBUG: OnGlobalsInitializeDebug(); break;
            case Globals.GlobalsType.NEIGHBORHOOD: OnGlobalsInitializeNeighborhood(); break;
            case Globals.GlobalsType.OBLIVION: OnGlobalsInitializeOblivion(); break;
        }
    }

    private void OnGlobalsInitializeDebug() {

    }

    private void OnGlobalsInitializeNeighborhood() {
        eyePoint = Utils.FindUniqueChildInTransform(transform, "EyeHeight");
    }

    private void OnGlobalsInitializeOblivion() {

    }
}
