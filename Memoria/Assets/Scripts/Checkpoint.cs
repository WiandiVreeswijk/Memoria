using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public Transform oblivionStopPoint;
    public Transform oblivionContinuePoint;
    public Transform respawnPoint;
    public CheckpointLantern lantern;

    private void OnTriggerEnter2D(Collider2D collision) {
        Globals.Player.CameraController.ZoomIn();
        if (lantern != null) lantern.Activate();
        Globals.CheckpointManager.SetCheckpoint(this);
        if (collision.CompareTag("Player")) {
            Globals.OblivionManager.SetNextSafeArea(oblivionStopPoint);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Globals.Player.CameraController.ZoomOut();
        if (collision.CompareTag("Player")) {
            Globals.OblivionManager.ContinueFromSaveArea(oblivionContinuePoint);
        }
    }

    public Vector3 GetRespawnPoint() {
        return respawnPoint.position;
    }

    public Vector3 GetOblivionStopPoint() {
        return oblivionStopPoint.position;
    }
}