using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDeath : MonoBehaviour {
    private float playerDeathDuration = 2.0f;
    private float recoverDuration = 0.5f;
    private bool isDying = false;
    Sequence deathDelaySequence;
    Sequence respawningSequence;

    private bool hasPlayed = false;

    private void FixedUpdate() {
        Globals.Screenshake.rumble = (1.0f - Mathf.Clamp(transform.position.x - Globals.OblivionManager.GetOblivionPosition(), 0.0f, 5.0f) / 5.0f) / 2.0f;
        if (Globals.OblivionManager.GetOblivionPosition() > transform.position.x) {
            if (deathDelaySequence == null) {
                Globals.SoundManagerChase.FadeDeath(1.0f, playerDeathDuration);
                Globals.Player.VisualEffects25D.FadeDeath(playerDeathDuration);
                deathDelaySequence = Utils.DelayedAction(playerDeathDuration, () => Respawn(false));
            }
        } else {
            if (deathDelaySequence != null) {
                Globals.SoundManagerChase.FadeDeath(0.0f, recoverDuration);
                Globals.Player.VisualEffects25D.CancelFadeDeath(recoverDuration);
                deathDelaySequence?.Kill();
                deathDelaySequence = null;
            }
        }

        if (transform.position.y < -2.0f) {
            if (!hasPlayed) {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFXChasingLevel/WaterSplash");
                hasPlayed = true;
            }
            deathDelaySequence?.Kill();
            deathDelaySequence = null;
            Respawn(true);
        }
    }

    private void Respawn(bool water) {
        if (!isDying) {
            isDying = true;
            Globals.AnalyticsManager.OnPlayerDeath(Globals.Player.transform.position, water);
            Globals.Screenshake.Shake(water ? 1.0f : 2.0f, water ? 0.2f : 0.5f);
            Globals.Player.PlayerMovement25D.SetStunned(true, true, true);
            Vector3 pos = Globals.Player.transform.position;
            pos.y = -1.0f; //Water position fix
            Globals.Player.VisualEffects25D.Death(water ? DeathType.WATER : DeathType.OBLIVION, pos);

            FMODUnity.RuntimeManager.PlayOneShot(water
                ? "event:/SFXChasingLevel/WaterSplash"
                : "event:/SFXChasingLevel/OblivionExplode");

            Globals.SoundManagerChase.FadeDeath(1.0f);
            respawningSequence = Utils.DelayedAction(2.0f, RespawnPosition);
            Utils.DelayedAction(1.0f, () => {
                Globals.Player.VisualEffects25D.isDead = false;
                Globals.MenuController.BlackScreenFadeIn(0.5f, false);
            });
        }
    }

    private void RespawnPosition() {
        Globals.MenuController.BlackScreenFadeOut(0.1f);
        Globals.Player.PlayerMovement25D.SetStunned(false, false, false);
        Globals.Player.VisualEffects25D.SetMeshEnabled(true);
        Globals.Player.VisualEffects25D.respawnParticles.Play();
        Globals.Player.VisualEffects25D.CancelFadeDeath(0.0f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFXChasingLevel/Player/Respawn");
        Globals.SoundManagerChase.FadeDeath(0.0f, 0.0f);
        Globals.OblivionVFXManager.ClearParticles();
        Globals.CheckpointManager.Respawn();
        Globals.Player.CameraController.SetCameraTargetPosition(Globals.Player.transform.position);
        deathDelaySequence?.Kill();
        deathDelaySequence = null;
        respawningSequence = null;
        isDying = false;
        hasPlayed = false;
    }

    public bool IsDying() {
        return isDying;
    }
}
