using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class PlayerTriggerVolume : MonoBehaviour {
    public PlayerTriggerVolumeType type;
    private AmbientControl sound;
    private Tween tween = null;

    private void Start() {
        sound = FindObjectOfType<AmbientControl>();
    }

    public enum PlayerTriggerVolumeType {
        EnterHouseCamera,
        LeaveHouseCamera,
        HouseInterior,
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            switch (type) {
                case PlayerTriggerVolumeType.EnterHouseCamera: Globals.Player.CameraController.ChangeCamera(PlayerCameraController.CameraType.FIRSTPERSON); break;
                case PlayerTriggerVolumeType.HouseInterior: HouseInterior(true); break;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            switch (type) {
                case PlayerTriggerVolumeType.LeaveHouseCamera: Globals.Player.CameraController.ChangeCameraAndSetRotation(PlayerCameraController.CameraType.THIRDPERSON, 180f, 0.65f); break;
                case PlayerTriggerVolumeType.HouseInterior: HouseInterior(false); break;
            }
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
