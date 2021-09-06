using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour {
    public void Remove() {
        Destroy(gameObject);
    }
}
public class IconManager : MonoBehaviour {
    [Serializable]
    public class IconDefinition {
        public string name;
        public Image image;

        public IconDefinition() {
            name = "";
            image = null;
        }
    }

    public List<IconDefinition> iconDefinitions = new List<IconDefinition>();

    public Icon AddWorldIcon() {
        return null;
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
