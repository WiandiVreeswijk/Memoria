using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts {
    public class MemoryEntrance : MonoBehaviour {
        public string sceneName;
        public CanvasGroup group;
        public List<Component> uiComponents;
        private bool open = true;
        public void OnActivateWatch() {
            open ^= true;
            if (open) Open();
            else Close();
        }

        private void Open() {
            group.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.InElastic);
            uiComponents.ForEach(x => {
                x.transform.DOScale(Vector3.one, 1.0f);
            });
        }

        private void Close() {
            group.transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InElastic);
            uiComponents.ForEach(x => {
                x.transform.DOScale(Vector3.one, 0.0f);
            });
        }

        public void Enter() {
            Globals.SceneManager.SetScene(sceneName);
        }
    }
}