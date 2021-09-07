using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour {
    public GameObject iconObject;
    public GameObject iconZObject;
    public float size = 1.0f;
    private RectTransform rt;

    public void SetImage(Texture texture) {
        Material mat = iconObject.GetComponent<MeshRenderer>().material;
        Material matZ = iconZObject.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_BaseMap", texture);
        matZ.SetTexture("_BaseMap", texture);
    }

    void Start() {
        rt = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        float scale = (Globals.Camera.transform.position - transform.position).magnitude * size;
        transform.localScale = new Vector3(scale, scale, scale);

        //Globals.Camera.ResetWorldToCameraMatrix();
        //var sp = Globals.Camera.WorldToScreenPoint(new Vector3(15, 3, 18));
        //print(sp.x + " + " + sp.y);
        //if (sp.z > 0) {
        //    rt.SetPositionAndRotation(new Vector3(sp.x, sp.y, 0), Quaternion.identity);
        //} else rt.position = new Vector2(-1000, -1000);
    }
}