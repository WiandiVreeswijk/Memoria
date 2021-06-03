using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    private Vector3 spawn;
    private Checkpoint lastCheckpoint;

    private void Start() {
        spawn = Globals.Player.transform.position;
    }

    public void SetCheckpoint(Checkpoint checkpoint) {
        lastCheckpoint = checkpoint;
    }

    public Checkpoint GetLastCheckpoint() {
        return lastCheckpoint;
    }

    public Vector3 GetSpawn() {
        return spawn;
    }
}
