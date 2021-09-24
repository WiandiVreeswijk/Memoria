using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrophyType {
    TIFA,
    BOAT
}

public class TrophyManager : MonoBehaviour {

    public List<Trophy> trophies = new List<Trophy>();
    public Material goldMaterial;
    public Material ghostMaterial;

    public void Start() {
        trophies.AddRange(GetComponentsInChildren<Trophy>());

        trophies.ForEach(x => x.SetMaterial(ghostMaterial));
    }
}
