using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using TMPro;

namespace Assets.Scripts {
    public class MemoryEntrance : MemoryObject {
        public string sceneName;
        public GameObject group;
        public GameObject portal;
        private int materialWidthProperty;
        private int materialHeightProperty;
        private int materialRimColorProperty;
        private Color rimColor;
        private Color rimColorNoAlpha;
        public RadialLayout radialLayout;
        private float radialDistance;
        private List<Transform> uiComponents = new List<Transform>();
        private bool open = true;
        public TextMeshProUGUI levelName;

        private Material portalMaterial;

        private List<Tween> tweens = new List<Tween>();

        public void Start() {
            portalMaterial = portal.GetComponent<MeshRenderer>().material;

            materialWidthProperty = Shader.PropertyToID("_Width");
            materialHeightProperty = Shader.PropertyToID("_Height");
            materialRimColorProperty = Shader.PropertyToID("_RimColor");
            rimColor = portalMaterial.GetColor(materialRimColorProperty);
            rimColorNoAlpha = new Color(rimColor.r, rimColor.g, rimColor.b, 0.0f);

            radialDistance = radialLayout.fDistance;
            for (int i = 0; i < group.transform.childCount; i++) {
                uiComponents.Add(group.transform.GetChild(i));
            }
        }

        public override void Activate()
        {
            Globals.Player.CameraController.EaseFOV(2.5f, 80f, true, Ease.InQuart);
            Globals.SceneManager.SetScene(sceneName);
        }

        private void KillTweens() {
            foreach (var tween in tweens) tween.Kill();
            tweens.Clear();
        }

        private Tween SetPortalSize(float size) {
            Tween tween = portalMaterial.DOFloat(size, materialWidthProperty, 1.0f).SetEase(Ease.OutExpo).OnUpdate(() => {
                float value = portalMaterial.GetFloat(materialWidthProperty);
                radialLayout.fDistance = radialDistance * value;
                radialLayout.CalculateRadial();
                portalMaterial.SetFloat(materialHeightProperty, value);
            });
            tweens.Add(levelName.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.1f));
            tweens.Add(tween);
            return tween;
        }

        private void Open() {
            KillTweens();
            tweens.Add(portalMaterial.DOColor(rimColor, materialRimColorProperty, 0.2f));
            uiComponents.ForEach(x => {
                tweens.Add(x.transform.DOScale(Vector3.one, 1.0f));
            });
            SetPortalSize(1.0f).OnComplete(() => {
                tweens.Add(levelName.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), 2.0f));
            });
            open = true;
        }

        private void Close() {
            KillTweens();
            uiComponents.ForEach(x => {
                tweens.Add(x.transform.DOScale(Vector3.zero, 0.1f));
            });
            SetPortalSize(0.0f);
            tweens.Add(portalMaterial.DOColor(rimColorNoAlpha, materialRimColorProperty, 1.2f).SetEase(Ease.InExpo));
            open = false;
        }

        public override void UpdateDistance(float distance) {
            if (distance < 3 && !open) {
                Open();
            } else if (distance > 5 && open) {
                Close();
            }
        }
    }
}