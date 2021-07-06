using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IEnterActivatable, IExitActivatable {
    public Transform oblivionStopPoint;
    public Transform oblivionContinuePoint;
    public Transform respawnPoint;
    public CheckpointLantern lantern;
    private int index = -1;

    public void SetCheckpointIndex(int index) {
        this.index = index;
    }

    public int GetCheckpointIndex() {
        return index;
    }

    public void OnRespawn() {
        lantern.EmitRespawnParticles();
    }

    public Vector3 GetRespawnPoint() {
        return respawnPoint.position;
    }

    public Vector3 GetOblivionStopPoint() {
        return oblivionStopPoint.position;
    }

    public void ActivateEnter() {
        Globals.CheckpointManager.SetCheckpoint(this);
        Globals.SoundManagerChase.FadeIntensity(0.0f);
    }

    public void ActivateExit() {
        if (!Globals.Player.PlayerDeath.IsDying()) {
            Globals.CheckpointManager.LeaveCheckpoint(this);
            Globals.SoundManagerChase.FadeIntensity(1.0f);
        }
    }
}