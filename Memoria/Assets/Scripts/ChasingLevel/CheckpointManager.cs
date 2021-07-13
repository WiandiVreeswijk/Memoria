using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    private Vector3 spawn;
    private int lastCheckpointIndex = -1;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private void Start() {
        spawn = Globals.Player.transform.position;

        checkpoints = new List<Checkpoint>(FindObjectsOfType<Checkpoint>());
        checkpoints.Sort((a, b) => {
            return (int)(a.transform.position.x - b.transform.position.x);
        });

        for (int i = 0; i < checkpoints.Count; i++) {
            checkpoints[i].SetCheckpointIndex(i);
        }
    }

    public void SetCheckpoint(Checkpoint checkpoint) {
        if (checkpoint.GetCheckpointIndex() > lastCheckpointIndex) {
            Globals.AnalyticsManager.OnCheckpointReached(checkpoint.GetCheckpointIndex());
            lastCheckpointIndex = checkpoint.GetCheckpointIndex();
            Globals.OblivionManager.SetGoalPosition(checkpoint.oblivionStopPoint.position.x, false);
        }
    }

    private Checkpoint GetCheckpointAtIndex(int index) {
        if (lastCheckpointIndex == -1 || index >= checkpoints.Count) return null;
        return checkpoints[index];
    }

    public Checkpoint GetLastCheckpoint() {
        return GetCheckpointAtIndex(lastCheckpointIndex);
    }

    public Checkpoint GetNextCheckpoint() {
        return GetCheckpointAtIndex(lastCheckpointIndex + 1);
    }

    public Vector3 GetSpawn() {
        return spawn;
    }

    public void Respawn() {
        Checkpoint lastCheckpoint = GetLastCheckpoint();
        if (lastCheckpoint != null) {
            Globals.Player.transform.position = lastCheckpoint.GetRespawnPoint();
            Globals.OblivionManager.SetOblivionPosition(lastCheckpoint.oblivionStopPoint.position.x);
            lastCheckpoint.OnRespawn();
        } else {
            Globals.Player.transform.position = Globals.CheckpointManager.GetSpawn();
        }
    }

    public bool FirstCheckpointReached() {
        return lastCheckpointIndex > -1;
    }

    public void LeaveCheckpoint(Checkpoint checkpoint) {
        if (checkpoint.GetCheckpointIndex() != lastCheckpointIndex)
            Debug.LogError($"Player left a checkpoint {checkpoint.GetCheckpointIndex()} without first entering it");

        Checkpoint nextCheckpoint = GetNextCheckpoint();
        Globals.OblivionManager.SetGoalPosition(GetLastCheckpoint().oblivionContinuePoint.position.x, true);
        if (nextCheckpoint != null) {
            Globals.OblivionManager.SetGoalPosition(nextCheckpoint.oblivionStopPoint.position.x, false);
        } else {
            Globals.OblivionManager.SetGoalPositionToEndPosition();
        }
    }
}
