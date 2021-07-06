using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldspaceObjectUITest : MonoBehaviour {
    private UnityEngine.UI.Image image;

    void LateUpdate() {
        if (image != null) {
            //Get the location of the UI element you want the 3d onject to move towards
            Vector3
                screenPoint =
                    image.transform.position +
                    new Vector3(0, 0.0f,
                        5); //the "+ new Vector3(0,0,5)" ensures that the object is so close to the camera you dont see it

            //find out where this is in world space
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);

            //move towards the world space position
            transform.position = Vector3.MoveTowards(transform.position, worldPos, 0.2f);
        }
    }

    public void Set(UnityEngine.UI.Image image1) {
        image = image1;
    }
}
