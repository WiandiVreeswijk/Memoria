using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInitializer : MonoBehaviour {
    public Globals.GlobalsType type;
    void OnEnable() {
        Globals.Initialize(type);
    }
}
