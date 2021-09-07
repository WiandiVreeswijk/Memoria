using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconTest : MonoBehaviour {
    // Start is called before the first frame update
    private RectTransform rt;
    void Start() {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float scale = (Globals.Camera.transform.position - transform.position).magnitude * 0.1f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
