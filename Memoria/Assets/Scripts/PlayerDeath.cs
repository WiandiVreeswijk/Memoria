using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDeath : MonoBehaviour {
    private bool isDying = false;
    Sequence deathDelaySequence;
    Sequence respawningSequence;

    private void FixedUpdate() {
        if (Globals.OblivionManager.GetOblivionPosition() > transform.position.x) {
            if (deathDelaySequence == null) deathDelaySequence = Utils.DelayedAction(1.0f, Respawn);
        } else {
            deathDelaySequence?.Kill();
            deathDelaySequence = null;
        }

        if (transform.position.y < -2.0f) {
            Respawn();
        }
    }

    private void Respawn() {
        if (!isDying) {
            isDying = true;
            VisualFX();
            respawningSequence = Utils.DelayedAction(1.0f, RespawnPosition);
        }
    }

    private void RespawnPosition() {
        Checkpoint lastCheckpoint = Globals.CheckpointManager.GetLastCheckpoint();
        if (lastCheckpoint == null) {
            Globals.Player.transform.position = Globals.CheckpointManager.GetSpawn();
            Globals.OblivionManager.SetDefaultOblivionPosition();
        } else {
            Globals.Player.transform.position = lastCheckpoint.GetRespawnPoint();
            Globals.OblivionManager.SetOblivionPosition(lastCheckpoint.GetOblivionStopPoint().x);
        }

        Globals.Player.VisualEffects.elenaMesh.enabled = true;
        Globals.Player.VisualEffects.respawnParticles.Play();
        Globals.OblivionVFXManager.ClearParticles();
        deathDelaySequence?.Kill();
        deathDelaySequence = null;
        respawningSequence = null;
        isDying = false;
    }

    private void VisualFX() {
        Globals.Player.VisualEffects.Death();
        Globals.Player.VisualEffects.isDeath = false;
    }

    public bool IsDying() {
        return isDying;
    }
}
