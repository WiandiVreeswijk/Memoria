using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChasingLevelUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI gemCollectibleCount;
    [SerializeField] private UnityEngine.UI.RawImage gemImage;

    private Vector3 scale;
    public void Start() {
        scale = gemImage.transform.localScale;
    }

    public void SetCollectibleCount(int count) {
        gemCollectibleCount.text = "" + count;
        gemImage.transform.localScale = new Vector3(scale.x * 1.5f, scale.y * 1.5f, 1.0f);
    }

    public void Update() {
        gemImage.transform.localScale = Vector3.Lerp(gemImage.transform.localScale, scale, Time.deltaTime * 2.0f);
    }

    public void SetEnabled(bool toggle) {
        gameObject.SetActive(toggle);
    }
}
