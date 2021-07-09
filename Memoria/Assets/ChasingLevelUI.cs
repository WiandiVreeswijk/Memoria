using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChasingLevelUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI gemCollectibleCount;

    public void SetCollectibleCount(int count) {
        gemCollectibleCount.text = "" + count;
    }
}
