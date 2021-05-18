using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAdventure : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] public float rotationSpeed = 5.0f;
    [SerializeField] public float timeToRunMultiplier = 1.0f;
    [SerializeField] public float movementDragMultiplier = 0.85f;
    [SerializeField] public float movementDecreaseWhenNotMoving = 0.01f;
    [SerializeField] public float startMovingTreshold = 0.25f;

    private Transform cam;
    private Animator animator;

    private Vector3 smoothedRotatedMovement = Vector3.zero;
    private Vector3 previousPlainMovement = Vector3.zero;
    private Vector3 plainRotatedMovement = Vector3.zero;
    private Vector3 previousMovement = Vector3.zero;
    private float runVariableSmoothed = 0.0f;
    private float forwardSpeed = 0.0f;
    private bool wasMovingLastFrame;

    private void Start() {
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update() {
        bool isRunning = false;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float rawHorizontal = Input.GetAxisRaw("Horizontal");
        float rawVertical = Input.GetAxisRaw("Vertical");
        Vector3 plainMovement = new Vector3(horizontal, 0, vertical); //This frame's movement store in a vector.
        Vector3 plainRawMovement = new Vector3(rawHorizontal, 0, rawVertical); //This frame's movement store in a vector.

        Vector3 movement = previousMovement * movementDragMultiplier; //Keep some movement speed from the previous frame.

        //Check if we're making a 180 turn
        float dot = Vector3.Dot(previousPlainMovement, plainMovement);
        bool sharpTurn = dot < 0.1f;
        if (sharpTurn) movement = Vector3.zero;
        if (plainRawMovement == Vector3.zero) { //No keys are pressed so no movement input
            if (!sharpTurn) {
                if (wasMovingLastFrame) runVariableSmoothed = 0.0f; //Player just stopped pressing movement keys
                forwardSpeed = Mathf.Clamp01(forwardSpeed - movementDecreaseWhenNotMoving); //Prevent instant movement stop
            }
            wasMovingLastFrame = false;
        } else if (sharpTurn || (Mathf.Abs(plainMovement.x) + Mathf.Abs(plainMovement.z)) > startMovingTreshold) {//If any keys are pressed and thus the player should be moving. Or if there is a sharp turn
            if (Input.GetKey(KeyCode.LeftShift)) isRunning = true;
            movement += plainMovement; //Add input movement to movement resulting from the previous frame

            Quaternion temporaryCameraRotation = cam.rotation;
            cam.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
            smoothedRotatedMovement = cam.TransformDirection(movement); //Movement rotated with the camera
            plainRotatedMovement = cam.TransformDirection(plainMovement); //Only used for debug
            cam.rotation = temporaryCameraRotation;

            if (sharpTurn) transform.rotation = Quaternion.LookRotation(smoothedRotatedMovement); //Instant turn
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(smoothedRotatedMovement), Time.deltaTime * rotationSpeed); //Smooth turn
            forwardSpeed = Mathf.Clamp01(smoothedRotatedMovement.magnitude);
            wasMovingLastFrame = true;
        }

        //Keep track of previous frame movement variables
        previousMovement = movement;
        previousPlainMovement = plainMovement;

        runVariableSmoothed = Mathf.Clamp01(runVariableSmoothed + ((isRunning ? timeToRunMultiplier : -timeToRunMultiplier) * Time.deltaTime)); //Gradually start running

        animator.SetFloat("Forward", forwardSpeed);
        animator.SetFloat("Running", runVariableSmoothed);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.ClampMagnitude(smoothedRotatedMovement, forwardSpeed)); //Smoother direction the player is moving to
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.ClampMagnitude(plainRotatedMovement, forwardSpeed)); //Direction the player is moving to
    }
}
