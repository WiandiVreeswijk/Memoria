using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldspaceUI : MonoBehaviour {

    public GameObject gameObject;
    public Image image;
    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            GameObject obj = Instantiate(gameObject, transform);
            obj.transform.position += new Vector3(0, 20, 100);
            obj.GetComponent<WorldspaceObjectUITest>().Set(image);
        }
        //transform.position = Camera.main.transform.position;
    }
}
