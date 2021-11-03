using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrophyType {
    TIFA,
    BOAT
}

public class TrophyManager : MonoBehaviour {
    private Dictionary<TrophyType, Trophy> trophies = new Dictionary<TrophyType, Trophy>();
    public Material goldMaterial;
    public Material ghostMaterial;

    public void OnGlobalsInitializeType(Globals.GlobalsType previousGlobalsType, Globals.GlobalsType currentGlobalsType) {
        if (currentGlobalsType == Globals.GlobalsType.NEIGHBORHOOD) {
            List<Trophy> trophiesList = new List<Trophy>(GetComponentsInChildren<Trophy>());
            trophies = Utils.ListToDictionary(trophiesList, "TrophyManager", x => x.GetTrophyType());
            trophiesList.ForEach(x => x.SetMaterial(ghostMaterial));
        }
    }

    private Trophy GetTrophy(TrophyType trophyType) {
        if (trophies.ContainsKey(trophyType)) return trophies[trophyType];
        Debug.LogError("Trophy not found in trophy cupboard: " + trophyType);
        return null;
    }

    public void CollectTrophy(TrophyType trophyType) {
        Trophy trophy = GetTrophy(trophyType);
        if (trophy != null) {
            trophy.SetMaterial(goldMaterial);
        }
    }
}
