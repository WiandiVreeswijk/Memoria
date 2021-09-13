using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts {
    public class MemoryEntrance : MonoBehaviour {
        public string sceneName;
        public GameObject group;
        public GameObject portal;
        private List<Transform> uiComponents;
        private bool open = true;

        private List<Tween> tweens = new List<Tween>();

        public void Start() {
            for (int i = 0; i < group.transform.childCount; i++) {
                uiComponents.Add(group.transform.GetChild(i));
            }
        }

        public void OnActivateWatch() {
            open ^= true;
            if (open) Open();
            else Close();
        }

        private void KillTweens() {
            foreach (var tween in tweens) tween.Kill();
            tweens.Clear();
        }

        private void Open() {
            KillTweens();
            tweens.Add(group.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.InElastic));
            uiComponents.ForEach(x => {
                tweens.Add(x.transform.DOScale(Vector3.one, 1.0f));
            });
        }

        private void Close() {
            KillTweens();
            tweens.Add(group.transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InElastic));
            uiComponents.ForEach(x => {
                tweens.Add(x.transform.DOScale(Vector3.one, 0.0f));
            });
        }

        public void Enter() {
            Globals.SceneManager.SetScene(sceneName);
        }

        public void FixedUpdate() {
            float distance = Vector3.Distance(Globals.Player.transform.position, transform.position);
            if (distance < 5 && !open) {
                Open();
            } else if (distance > 5 && open) {
                Close();
            }
        }
    }
}