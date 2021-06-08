using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    public SkinnedMeshRenderer elenaMesh;
    public ParticleSystem deathParticles;

    public ParticleSystem[] dustParticleSystems;

    [HideInInspector]
    public bool isDeath = false;
    private bool isBlinking = false;
    private Material playerMaterial;
    public Color stunnedColor = Color.white;


    private void Start()
    {
        playerMaterial = elenaMesh.material;
        deathParticles.Stop();
    }
    #region Blinking

    public void DoBlink(float speed, int times)
    {
        if (isBlinking) return;
        isBlinking = true;
        playerMaterial.DOColor(stunnedColor, 0.1f);
        BlinkOff(speed, times);
    }

    void BlinkOn(float speed, int times)
    {
        elenaMesh.enabled = true;
        if (times > 0) Utils.DelayedAction(speed, () => BlinkOff(speed, times));
        else
        {
            isBlinking = false;
            playerMaterial.DOColor(Color.white, 0.1f);
        }

    }

    void BlinkOff(float speed, int times)
    {
        elenaMesh.enabled = false;
        Utils.DelayedAction(speed, () => BlinkOn(speed, times - 1));
    }

    #endregion

    #region Death
    public void Death()
    {
        if (!isDeath)
        {
            elenaMesh.enabled = false;
            deathParticles.Play();
            isDeath = true;
        }

    }

    #endregion

    #region Dust
    public void Jump(float direction)
    {
        PlayDust(0.075f, 200, direction);
    }
    public void Land(float direction)
    {
        PlayDust(0.075f, 100, direction);
    }
    private void PlayDust(float duration, int count, float velocity)
    {
        ParticleSystem particleSystem = null;
        foreach (var ps in dustParticleSystems)
        {
            if (ps.isStopped)
            {
                particleSystem = ps;
                continue;
            }
        }

        if (particleSystem != null)
        {
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
}
