using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    public SkinnedMeshRenderer elenaMesh;
    public ParticleSystem deathParticles;

    private void Start()
    {
        deathParticles.Stop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Death();
        }
    }

    public void DoBlink(float speed, int times)
    {
        BlinkOff(speed, times);
    }

    void BlinkOn(float speed, int times)
    {
        elenaMesh.enabled = true;
        if (times > 0) Utils.DelayedAction(speed, () => BlinkOff(speed, times));
    }

    void BlinkOff(float speed, int times)
    {
        elenaMesh.enabled = false;
        Utils.DelayedAction(speed, () => BlinkOn(speed, times - 1));
    }

    void Death()
    {
        elenaMesh.enabled = false;
        deathParticles.Play();

    }
}
