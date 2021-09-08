using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour {
    [Serializable]
    public class IconDefinition {
        public string name;
        public Texture texture;

        public IconDefinition() {
            name = "";
            texture = null;
        }
    }

    public List<IconDefinition> iconDefinitions = new List<IconDefinition>();
    public Dictionary<string, IconDefinition> iconDefinitionsDict = new Dictionary<string, IconDefinition>();
    public GameObject iconPrefab;

    public void Awake() {
        foreach (IconDefinition def in iconDefinitions) {
            if (iconDefinitionsDict.ContainsKey(def.name)) {
                Debug.LogError("Duplicate icon definition found in IconManager: " + name);
            } else {
                iconDefinitionsDict.Add(def.name, def);
            }
        }
    }

    public Icon AddWorldIcon(string name, Vector3 position) {
        
        if (iconDefinitionsDict.TryGetValue(name, out IconDefinition def)) {
            Icon icon = Instantiate(iconPrefab, position, Quaternion.identity).GetComponent<Icon>();
            icon.transform.parent = Globals.UIManager.OverlayCanvas.transform;
            icon.SetImage(def.texture);
            return icon;
        }
        Debug.LogError("IconDefinition " + name + " not found in IconManager");
        return null;
    }
}
