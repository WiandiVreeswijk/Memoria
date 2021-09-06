using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconTest : MonoBehaviour {
    // Start is called before the first frame update
    private RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 sp = Globals.Camera.WorldToScreenPoint(new Vector3(15, 3, 18));
        print(sp);
        rt.position= new Vector3(sp.x, sp.y, 0);
    }
}
