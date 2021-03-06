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
    public GameObject dialogueIconPrefab;

    public void Awake() {
        iconDefinitionsDict = Utils.ListToDictionary(iconDefinitions, "IconManager", x => x.name);
    }

    public Icon AddWorldIcon(string name, Vector3 position) {
        if (iconDefinitionsDict.TryGetValue(name, out IconDefinition def)) {
            Icon icon = Instantiate(iconPrefab, position, Quaternion.identity).GetComponent<Icon>();
            icon.transform.parent = transform;
            icon.SetImage(def.texture);
            return icon;
        }
        Debug.LogError("IconDefinition " + name + " not found in IconManager");
        return null;
    }
}
