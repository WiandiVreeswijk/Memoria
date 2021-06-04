using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//calculates collision with a point and a circle & resolves collision
public class CircleCollision : MonoBehaviour {

    //circles radius
    public float radius;
    public new Camera camera;

    bool isMoved;



    // Use this for initialization
    void Start() {
        //set the radius based off the objects size
        //radius = gameObject.transform.localScale.x / 2;
        camera = Camera.main;

    }

    // Update is called once per frame
    void Update() {
        //if the object is clicked move it
        if (isMoved) {
            gameObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y, 0);

        }
    }

    public void checkCollision(GameObject node) {
        // if the difference in position in x or y is greater than the radius return
        //used for simple optomization to prevent the need of magnitude checks
        if (Mathf.Abs(node.transform.position.x - gameObject.transform.position.x) > radius || Mathf.Abs(node.transform.position.y - gameObject.transform.position.y) > radius) {
            return;
        }
        //if the dist between the centers is less than the radius resolve collison
        Vector3 direction = node.transform.position - gameObject.transform.position;
        float magnitude = Vector3.Magnitude(direction);
        if (magnitude <= radius) {
            //get the direction for the angle between the objects
            direction = Vector3.Normalize(direction);
            //get how deeep the point penetrated into the object
            float moveMag = radius - magnitude;
            //multiply the direction by the mangitude of movement to get the final positon
            direction *= moveMag;
            //update the objects position
            node.transform.position += direction;
            //multiply the velocity by .7 to deal with friction
            node.GetComponent<ClothNode>().velocity *= .7f;
        }
        return;
    }


    //mouse controll methods
    private void OnMouseDown() {


        isMoved = true;
        gameObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y, 0);


    }

    private void OnMouseUp() {
        isMoved = false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
