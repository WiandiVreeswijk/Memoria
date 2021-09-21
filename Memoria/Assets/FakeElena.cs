using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeElena : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            SetLookAt(new Vector3(-1000, 0.0f, 0.0f));
        } else if (Input.GetKeyDown(KeyCode.F)) {
            SetLookAt(new Vector3(1000, 0.0f, 0.0f));
        }
    }

    public void SetLookAt(Vector3? position) {
        if (position.HasValue) {
            print(position.Value);
            GetComponent<Animator>().SetLookAtPosition(position.Value);
            GetComponent<Animator>().SetLookAtWeight(1f);
        } else {
            GetComponent<Animator>().SetLookAtWeight(0f);
        }
    }
}
