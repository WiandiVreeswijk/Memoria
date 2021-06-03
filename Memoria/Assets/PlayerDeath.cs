using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    private bool isRespawning = false;
    private float offset = 3;

    private void FixedUpdate() {
        if (!isRespawning && Globals.OblivionManager.GetOblivionPosition() - offset > transform.position.x || transform.position.y < -2.0f) {
            isRespawning = true;
            Invoke("Respawn", 0.0f);
        }
    }

    private void Respawn() {
        Checkpoint lastCheckpoint = Globals.CheckpointManager.GetLastCheckpoint();
        if (lastCheckpoint == null) {
            Globals.Player.transform.position = Globals.CheckpointManager.GetSpawn();
            Globals.OblivionManager.SetOblivionPosition(-5);
        } else {
            Globals.Player.transform.position = lastCheckpoint.GetRespawnPoint();
            Globals.OblivionManager.SetOblivionPosition(lastCheckpoint.GetOblivionStopPoint().x);
        }

        Globals.Player.VisualEffects.Death();
        Globals.Player.VisualEffects.isDeath = false;
        Globals.Player.VisualEffects.elenaMesh.enabled = true;
        isRespawning = false;
    }
}
