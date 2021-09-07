using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour {
    public GameObject iconObject;
    public GameObject iconZObject;
    private RectTransform rt;

    public void SetImage(Texture texture) {
        Image image = iconObject.GetComponent<Image>();
        Image imageZ = iconZObject.GetComponent<Image>();
        image.material.mainTexture = texture;
        imageZ.material.mainTexture = texture;
    }

    void Start() {
        rt = GetComponent<RectTransform>();
    }
    void LateUpdate() {
        Globals.Camera.ResetWorldToCameraMatrix();
        var sp = Globals.Camera.WorldToScreenPoint(new Vector3(15, 3, 18));
        print(sp.x + " + " + sp.y);
        if (sp.z > 0) {
            rt.SetPositionAndRotation(new Vector3(sp.x, sp.y, 0), Quaternion.identity);
        } else rt.position = new Vector2(-1000, -1000);
    }
}