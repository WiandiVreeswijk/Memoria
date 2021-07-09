using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using DG.Tweening;

public class GemCollectible : MonoBehaviour, IEnterActivatable {
    [EventRef]
    public string soundEffect = "";
    public GameObject visualPrefab;
    [Range(1, 100)] public int points = 1;

    private ParticleSystem pSystem;
    private Animator animator;

    public void Start() {
        pSystem = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        animator.Play("GemAnimation", 0, ((transform.position.x % 10) / 5.0f) - 1.0f);
    }

    public void ActivateEnter() {
        animator.enabled = false;
        Utils.RepeatAction(points, 0.1f, () => EmitVisual(visualPrefab, soundEffect));
        Destroy(gameObject);
    }

    static void EmitVisual(GameObject visualPrefab, string soundEffect) {
        GameObject instantiated = Instantiate(visualPrefab);
        instantiated.transform.position = new Vector3(Globals.Player.HeadPosition.x, Globals.Player.HeadPosition.y + 0.5f, Globals.Player.HeadPosition.z);
        instantiated.transform.parent = Globals.UIManager.ScreenspaceCanvas.transform;
        FMODUnity.RuntimeManager.PlayOneShot(soundEffect);
    }

    public void PlayPFX() {
        pSystem.Play();
    }
}