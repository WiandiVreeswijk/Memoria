using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class PlayerTriggerVolume : MonoBehaviour {
    public PlayerTriggerVolumeType type;
    public GameObject elenaMesh;
    public CinemachineVirtualCamera firstPersonCamera;
    private PlayerMovementAdventure adventureMovement;
    private AmbientControl sound;
    private Tween tween = null;

    private void Start() {
        adventureMovement = FindObjectOfType<PlayerMovementAdventure>();
        sound = GameObject.FindObjectOfType<AmbientControl>();
    }

    public enum PlayerTriggerVolumeType {
        EnterHouseCamera,
        LeaveHouseCamera,
        HouseInterior,
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            switch (type) {
                case PlayerTriggerVolumeType.EnterHouseCamera: HouseCamera(true); break;
                case PlayerTriggerVolumeType.HouseInterior: HouseInterior(true); break;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            switch (type) {
                case PlayerTriggerVolumeType.LeaveHouseCamera: HouseCamera(false); break;
                case PlayerTriggerVolumeType.HouseInterior: HouseInterior(false); break;
            }
        }
    }

    private void HouseCamera(bool enter) {
        if (enter) {
            adventureMovement.inHouse = true;
            firstPersonCamera.Priority = 11;
            elenaMesh.SetActive(false);
        } else {
            adventureMovement.inHouse = false;
            firstPersonCamera.Priority = 9;
            elenaMesh.SetActive(true);
        }
    }

    private void HouseInterior(bool enter) {
        tween?.Kill();
        if (enter) {
            tween = DOTween.To(() => sound.GetVolume(), x => sound.SetVolume(x), 0, 2.0f);
        } else {
            tween = DOTween.To(() => sound.GetVolume(), x => sound.SetVolume(x), 0.7f, 2.0f);
            tween = DOTween.To(() => sound.GetBirds(), x => sound.SetBirds(x), 0.7f, 2.0f);
        }
    }
}
