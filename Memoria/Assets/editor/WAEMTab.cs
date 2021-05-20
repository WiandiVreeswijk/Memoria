using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WAEMTab {
    public abstract void Initialize();
    public abstract void OnGUI(GUIStyle style);
    public abstract void OnUpdate();
}
