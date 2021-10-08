using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour {
    [SerializeField] private GameObject indicatorPrefab;

    public OffScreenIndicator CreateIndicator(GameObject target) {
        OffScreenIndicator indicator = Instantiate(indicatorPrefab, transform).GetComponent<OffScreenIndicator>();
        indicator.target = target;
        return indicator;
    }
}
