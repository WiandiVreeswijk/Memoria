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

    public void Start() {
        foreach (IconDefinition def in iconDefinitions)
        {
            string name = def.name.ToLower();
            if (iconDefinitionsDict.ContainsKey(name)) {
                Debug.LogError("Duplicate icon definition found in IconManager: " + name);
            } else {
                iconDefinitionsDict.Add(name, def);
            }
        }
    }

    public Icon AddWorldIcon(string name, Vector3 position)
    {

        name = name.ToLower();
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
