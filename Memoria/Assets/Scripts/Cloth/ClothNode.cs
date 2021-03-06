using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//a script contianing all the info about each point in world space
public class ClothNode : MonoBehaviour {

    public Vector3 originalPosition;
    //basic movement info
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Vector3 force;

    //objects mass
    public float mass;

    //an anchor object is locked in place and cant be moved
    public bool isAnchor;

    //the wall in the scene
    public GameObject walls;

    //the circle in the scene
    public List<CircleCollision> spheres;

    //if the node is drawn
    public bool draw;

    //mouse movement
    public bool isMoved;

    //limit speed and force
    float maxSpeed;
    public float maxForce;

    //vector for gravity
    public Vector3 gravity;



    // Use this for initialization
    void Start() {
        originalPosition = transform.position;
        //set all the basic default values
        maxForce = 100000;
        draw = true;
        maxSpeed = 100f;

        //if the object is an anchor set its color to blue
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (isAnchor) {
            renderer.material.color = Color.blue;
        } else {
            renderer.material.color = Color.white;
        }

    }

    private float avg = 100;
    public void beginSim() {

        //get poistion and make sure mass > 0
        position = gameObject.transform.position;
        if (mass <= 0) {
            mass = 1.0f;
        }

        gravity = new Vector3(0, -9.8f * mass / 4, 0);
    }

    // Update is called once per frame
    public void FixedUpdate() {
        //wait for 1 second so every node in the fabric can be loaded in
        //set postion to the actual poistion
        position = gameObject.transform.position;
        Vector3 oPosition = position;
        if (!isAnchor) {

            //basic euiler ingegration for movement
            acceleration = force / mass;
            acceleration = Vector3.ClampMagnitude(acceleration, maxSpeed * 10);
            velocity += acceleration * Time.deltaTime;

            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            //apply friction
            velocity *= .99f; // - ((velocity.magnitude / maxSpeed) * .05f);
            position += velocity * Time.deltaTime;


            gameObject.transform.position = position;
            //if the wall is enableled calculate the collision
            //chec if the circle is active in the heirarchy and resolve collsion
            foreach (var sphere in spheres) {
                sphere.checkCollision(this.gameObject);
            }

            avg += Vector3.Distance(transform.position, oPosition);
            avg /= 2;
            if (avg < 0.0001f) {
                SetColor(Color.blue);
                isAnchor = true;
            }
            //reset the force
            force = gravity;
        }
    }

    //set the object as an anchor & change the sprite color
    void changeAnchor() {
        isAnchor = !isAnchor;
        SpriteRenderer mySpirite = GetComponent<SpriteRenderer>();
        if (isAnchor) {
            mySpirite.color = Color.blue;
        } else {
            mySpirite.color = Color.white;
        }
    }

    //swap if the object is drawn if its not disable thje sprite
    public void drawNode() {
        draw = !draw;
        if (!draw) {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        } else {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    //turn gravity on and off
    public void toggleGravity(bool change) {
        if (change) {
            gravity = new Vector3(0, -9.8f * mass / 4, 0);
        } else {
            gravity = Vector3.zero;
        }
    }

    // increase the force by an ammount but clamp the magnitude
    public void applyForce(Vector3 newForce) {
        force += newForce;
        force = Vector3.ClampMagnitude(force, maxForce);
    }

    public void SetColor(Color color) {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.color = color;
    }
}
