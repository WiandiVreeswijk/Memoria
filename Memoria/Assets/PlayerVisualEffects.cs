using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour {
    public SkinnedMeshRenderer elenaMesh;
    public ParticleSystem deathParticles;

    private Tween tween = null;
    public ParticleSystem dust;

    [HideInInspector]
    public bool isDeath = false;
    private bool isBlinking = false;

    private void Start() {
        deathParticles.Stop();
    }
    #region Blinking

    public void DoBlink(float speed, int times) {
        if (isBlinking) return;
        isBlinking = true;
        BlinkOff(speed, times);
    }

    void BlinkOn(float speed, int times) {
        elenaMesh.enabled = true;
        if (times > 0) Utils.DelayedAction(speed, () => BlinkOff(speed, times));
        else isBlinking = false;
    }

    void BlinkOff(float speed, int times) {
        elenaMesh.enabled = false;
        Utils.DelayedAction(speed, () => BlinkOn(speed, times - 1));
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
    public void Jump()
    {
        PlayDust(0.075f, 200);
    }
    public void Land()
    {
        PlayDust(0.25f, 100);
    }
    private void PlayDust(float duration, int count)
    {
        if (!dust.isStopped) return;
        var m = dust.main;
        m.duration = duration;
        var e = dust.emission;
        e.rateOverTime = count;
        dust.Play();
    }
    #endregion
}
