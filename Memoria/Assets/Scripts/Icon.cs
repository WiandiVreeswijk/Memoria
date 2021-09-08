using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour {
    public GameObject iconObject;
    public GameObject iconZObject;
    public TextMesh distanceText;
    public TextMesh distanceTextZ;
    public float size = 1.0f;
    public float minScale = 1.0f;
    public float maxScale = 1.0f;

    public void SetImage(Texture texture) {
        Material mat = iconObject.GetComponent<MeshRenderer>().material;
        Material matZ = iconZObject.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_BaseMap", texture);
        matZ.SetTexture("_BaseMap", texture);
    }

    public void SetText(string text) {
        distanceText.text = text;
        distanceTextZ.text = text;
    }

    void LateUpdate() {
        SetText(Mathf.Round(Vector3.Distance(Globals.Camera.transform.position, transform.position)) + "M");
        float scale = (Globals.Camera.transform.position - transform.position).magnitude * size;
        scale = Mathf.Clamp(scale, minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);

        //Globals.Camera.ResetWorldToCameraMatrix();
        //var sp = Globals.Camera.WorldToScreenPoint(new Vector3(15, 3, 18));
        //print(sp.x + " + " + sp.y);
        //if (sp.z > 0) {
        //    rt.SetPositionAndRotation(new Vector3(sp.x, sp.y, 0), Quaternion.identity);
        //} else rt.position = new Vector2(-1000, -1000);
    }
}