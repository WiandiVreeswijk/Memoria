using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Renderer checkpointMaterial;
    public Color activateColor, inactiveColor;
    private bool isActivated = false;

    private PlayerRespawn respawn;
    private PlayerDeath death;

    public ParticleSystem particles;
    public GameObject checkpoint;

    private Tween tween;
    public Vector3 punch;
    public float duration = 0.5f, elasticity = 0.3f;
    public int vibrato = 1;

    private void Start()
    {
        respawn = FindObjectOfType<PlayerRespawn>();
        death = FindObjectOfType<PlayerDeath>();
        particles.Stop();
        NotActivated();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            Activated();
            isActivated = true;
        }
    }

    private void Activated()
    {
        checkpointMaterial.material.color = activateColor;
        respawn.playerStartPosition = transform.position;
        death.playerStartPosition = transform.position;
        tween = checkpoint.transform.DOPunchScale(punch, duration, vibrato, elasticity);
        particles.Play();
    }

    private void NotActivated()
    {
        checkpointMaterial.material.color = inactiveColor;
    }
}

