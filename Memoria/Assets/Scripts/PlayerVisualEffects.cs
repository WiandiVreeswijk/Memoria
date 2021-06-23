using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour {
    public SkinnedMeshRenderer elenaMesh;
    public ParticleSystem deathParticles;

    public ParticleSystem[] dustParticleSystems;

    public ParticleSystem respawnParticles;

    [HideInInspector]
    public bool isDeath = false;
    private bool isBlinking = false;
    private Material playerMaterial;
    private int oblivionPositionPropertyID;
    public Color stunnedColor = Color.white;
    private Tween fadeDeathTween;

    private void Start() {
        playerMaterial = elenaMesh.material;
        oblivionPositionPropertyID = Shader.PropertyToID("_OblivionPosition");
        deathParticles.Stop();
        CancelFadeDeath(0.0f);
    }
    #region Blinking

    public void DoBlink(float speed, int times) {
        if (isBlinking) return;
        isBlinking = true;
        playerMaterial.DOColor(stunnedColor, 0.1f);
        BlinkOff(speed, times);
    }

    void BlinkOn(float speed, int times) {
        elenaMesh.enabled = true;
        if (times > 0) Utils.DelayedAction(speed + speed / 2.0f, () => BlinkOff(speed, times));
        else {
            isBlinking = false;
            playerMaterial.DOColor(Color.white, 0.1f);
        }
    }

    void BlinkOff(float speed, int times) {
        elenaMesh.enabled = false;
        Utils.DelayedAction(speed / 2.0f, () => BlinkOn(speed, times - 1));
    }

    #endregion

    #region Death
    public void Death() {
        if (!isDeath) {
            elenaMesh.enabled = false;
            deathParticles.Play();
            isDeath = true;
        }

    }

    #endregion

    #region Dust
    public void Jump(float direction) {
        PlayDust(0.075f, 200, direction);
    }
    public void Land(float direction) {
        PlayDust(0.075f, 100, direction);
    }
    private void PlayDust(float duration, int count, float velocity) {
        ParticleSystem particleSystem = null;
        foreach (var ps in dustParticleSystems) {
            if (ps.isStopped) {
                particleSystem = ps;
                continue;
            }
        }

        if (particleSystem != null) {
            var m = particleSystem.main;
            m.duration = duration;
            var e = particleSystem.emission;
            e.rateOverTime = count;
            var l = particleSystem.velocityOverLifetime;
            float force = -velocity / 4.0f;
            l.x = force;
            particleSystem.Play();
        }
    }
    #endregion

    #region Oblivion

    private void FadeOblivion(float value, float duration) {
        fadeDeathTween?.Kill();
        if (duration == 0.0f) playerMaterial.SetFloat(oblivionPositionPropertyID, value);
        else {
            fadeDeathTween = DOTween.To(() => playerMaterial.GetFloat(oblivionPositionPropertyID),
                x => playerMaterial.SetFloat(oblivionPositionPropertyID, x), value, duration).SetEase(Ease.OutQuad);
        }
    }

    public void FadeDeath(float duration) {
        FadeOblivion(0.5f, duration);
    }

    public void CancelFadeDeath(float duration) {
        FadeOblivion(-2.0f, duration);
    }
    #endregion
}
