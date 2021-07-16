using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PortalVisual : MonoBehaviour {
    private bool isOpen = false;
    private Material material;
    public GameObject player;

    void Start() {
        material = GetComponent<MeshRenderer>().material;
        material.SetFloat("_Width", 0);
        material.SetFloat("_Height", 0);
        material.SetFloat("_Offset", 0.5f);
    }

    public void Update() {
        if (isOpen) {
            if (player != null && player.transform.position.z < transform.position.z) {
                material.SetFloat("_Offset", 3f);
            }
        }
    }

    public void SetOpen(bool open) {
        if (open) Open();
        else Close();
    }

    public void Open() {
        Utils.DelayedAction(0.75f, () => Width(1.0f, 1f));
        Height(2.0f, 1.0f).SetEase(Ease.InElastic).OnComplete(() => isOpen = true);
    }

    public void Close() {
        isOpen = false;
        Utils.DelayedAction(0.5f, () => Width(0.0f, 1f));
        Height(0.0f, 1.5f).SetEase(Ease.InElastic);
    }

    private Tween Width(float goal, float time) {
        return DOTween.To(() => material.GetFloat("_Width"), x => material.SetFloat("_Width", x), goal, time);
    }

    private Tween Height(float goal, float time) {
        return DOTween.To(() => material.GetFloat("_Height"), x => material.SetFloat("_Height", x), goal, time);
    }

    public bool IsOpen() {
        return isOpen;
    }
}
