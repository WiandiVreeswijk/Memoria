using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour {
    public GameObject iconObject;
    public GameObject iconZObject;

    public void SetImage(Texture texture) {
        Material material = iconObject.GetComponent<MeshRenderer>().material;
        Material materialZ = iconZObject.GetComponent<MeshRenderer>().material;
        material.SetTexture("_BaseMap", texture);
        materialZ.SetTexture("_BaseMap", texture);
    }

    public void Update()
    {
        Vector2 point = Globals.Camera.WorldToScreenPoint(transform.position);
        transform.position = point;
    }
}
