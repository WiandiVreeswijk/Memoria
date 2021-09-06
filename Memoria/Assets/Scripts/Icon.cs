using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour {
    public GameObject iconObject;
    public GameObject iconZObject;
    private RectTransform rt;

    public void SetImage(Texture texture) {
        Material material = iconObject.GetComponent<MeshRenderer>().material;
        Material materialZ = iconZObject.GetComponent<MeshRenderer>().material;
        material.SetTexture("_BaseMap", texture);
        materialZ.SetTexture("_BaseMap", texture);
    }

    void Start() {
        rt = GetComponent<RectTransform>();
    }
    void LateUpdate() {
        Globals.Camera.ResetWorldToCameraMatrix();
        var sp = Globals.Camera.WorldToScreenPoint(new Vector3(15, 3, 18));
        if (sp.z > 0) {
            sp.z = 0;
            rt.SetPositionAndRotation(new Vector3(sp.x, sp.y, 0), Quaternion.identity);
        } else rt.position = new Vector2(-1000, -1000);
    }
}
