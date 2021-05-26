using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script that controlls the spring that connects two nodes together
public class ClothSpring : MonoBehaviour {

    //the two nodes connected
    public ClothNode node1;
    public ClothNode node2;
    //the resting size of the spring
    float restingSize;
    //a bool for drawing the spring
    public bool hasLines = true;

    float springStrength;

    // Use this for initialization
    void Start() {

    }

    //set the resting size based of the starting position and set the streingth of the spring
    public void startPlay(float startSize, float newSpringStrength) {
        hasLines = true;
        restingSize = Vector3.Magnitude(node1.transform.position - node2.transform.position) * startSize;
        springStrength = newSpringStrength;
    }



    // Update is called once per frame
    public void FixedUpdate() {
        //wait 1 second after everything is placed in the scene
        //get the scripts attached to the points
        //get the diference in velocity
        Vector3 deltaV = node1.velocity - node2.velocity;

        //set the spring evenly between the two points this gives us the direction for the spring
        // Vector3 springPos = Vector3.Normalize( point1.position - point2.position);
        Vector3 springPos = node1.position - node2.position;
        //get the current size of the spring
        float springCurrent = Vector3.Magnitude(node1.position - node2.position);

        //calculate the force based off of hooks law
        //SpringStrength = K springCurrent - restingSize = S an
        Vector3 springForce = -springStrength * (springCurrent - restingSize) * springPos;

        //set the dampening of the spring based off the strength of the spring instead of another dampening value
        springForce += (float)-springStrength / 100f * Vector3.Dot(deltaV, springPos) * springPos;

        //if the points aren't anchors apply the forces
        if (!node1.isAnchor) {
            node1.applyForce(springForce);
        }
        if (!node2.isAnchor) {
            node2.applyForce(-springForce);
        }

        //if lines need to be rendered render the lines
        if (hasLines) {
            GetComponent<LineRenderer>().enabled = true;
            GetComponent<LineRenderer>().SetPositions(new Vector3[2] { node1.transform.position, node2.transform.position });
            if (springCurrent > restingSize) {
                //if the size of the springs is > the resting size make the spring colors red
                GetComponent<LineRenderer>().startColor = Color.red;
                GetComponent<LineRenderer>().endColor = Color.red;
            } else {
                //otherwise make set them to green
                GetComponent<LineRenderer>().startColor = Color.green;
                GetComponent<LineRenderer>().endColor = Color.green;
            }
        } else {
            GetComponent<LineRenderer>().enabled = false;
        }
    }
}
