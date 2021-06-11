using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDeath : MonoBehaviour {
    Sequence deathDelay;
    private void FixedUpdate() {
        if (Globals.OblivionManager.GetOblivionPosition() > transform.position.x) {
            if (deathDelay == null) deathDelay = Utils.DelayedAction(1.0f, Respawn);
        } else {
            deathDelay?.Kill();
            deathDelay = null;
        }

        if (transform.position.y < -2.0f) {
            Respawn();
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
        deathDelay?.Kill();
        deathDelay = null;
    }
}
