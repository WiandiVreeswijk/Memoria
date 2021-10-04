using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovementAdventure : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] public float rotationSpeed = 5.0f;
    [SerializeField] public float timeToRunMultiplier = 1.0f;
    [SerializeField] public float movementDragMultiplier = 0.85f;
    [SerializeField] public float movementDecreaseWhenNotMoving = 0.01f;
    [SerializeField] public float startMovingTreshold = 0.25f;

    public Camera cam;
    private Animator animator;

    private Vector3 smoothedRotatedMovement = Vector3.zero;
    private Vector3 previousPlainMovement = Vector3.zero;
    private Vector3 plainRotatedMovement = Vector3.zero;
    private Vector3 previousMovement = Vector3.zero;
    private float runVariableSmoothed = 0.0f;
    private float forwardSpeed = 0.0f;
    private bool wasMovingLastFrame;

    private CharacterController controller;
    public float fpsSpeed = 2.5f;
    private float gravity = 9.81f;

    private bool hasMoved = false;
    private bool canMove = true;

    private void Start() {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (!Globals.Player.CameraController.IsInFirstPerson()) {
            animator.applyRootMotion = true;
            Adventure();
        } else {
            animator.applyRootMotion = false;
            if (FPSLookAt.sqrMagnitude > 0.01f && canMove)
                gameObject.transform.rotation = Quaternion.LookRotation(FPSLookAt, Vector3.up);
            FPS();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.ClampMagnitude(smoothedRotatedMovement, forwardSpeed)); //Smoother direction the player is moving to
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.ClampMagnitude(plainRotatedMovement, forwardSpeed)); //Direction the player is moving to
    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }
    public bool HasMoved() { return hasMoved; }

    public void Teleport(Vector3 position) {
        Teleport(position, null);
    }

    public void Teleport(Vector3 position, Quaternion? rotation) {
        Utils.DelayedAction(0, () => {
            transform.position = position;
            if (rotation.HasValue) {
                transform.rotation = rotation.Value;
            }
        }).SetUpdate(UpdateType.Late);
    }

    private void Adventure() {
        bool isRunning = true;
        float horizontal = canMove ? Input.GetAxis("Horizontal") : 0.0f;
        float vertical = canMove ? Input.GetAxis("Vertical") : 0.0f;
        float rawHorizontal = canMove ? Input.GetAxisRaw("Horizontal") : 0.0f;
        float rawVertical = canMove ? Input.GetAxisRaw("Vertical") : 0.0f;

        Vector3 plainMovement = new Vector3(horizontal, 0, vertical); //This frame's movement store in a vector.
        Vector3 plainRawMovement = new Vector3(rawHorizontal, 0, rawVertical); //This frame's movement store in a vector.

        Vector3 movement = previousMovement * movementDragMultiplier; //Keep some movement speed from the previous frame.

        //Check if we're making a 180 turn
        float dot = Vector3.Dot(previousPlainMovement, plainMovement);
        bool sharpTurn = dot < 0.1f && dot != 0.0f;
        if (sharpTurn) movement = Vector3.zero;
        if (plainRawMovement == Vector3.zero) { //No keys are pressed so no movement input
            if (!sharpTurn) {
                if (wasMovingLastFrame) runVariableSmoothed = 0.0f; //Player just stopped pressing movement keys
                forwardSpeed = Mathf.Clamp01(forwardSpeed - movementDecreaseWhenNotMoving); //Prevent instant movement stop
            }
            wasMovingLastFrame = false;
        } else if (sharpTurn || (Mathf.Abs(plainMovement.x) + Mathf.Abs(plainMovement.z)) > startMovingTreshold) {//If any keys are pressed and thus the player should be moving. Or if there is a sharp turn
            if (Input.GetButton("Sprint")) isRunning = false;
            movement += plainMovement; //Add input movement to movement resulting from the previous frame

            Quaternion temporaryCameraRotation = cam.transform.rotation;
            cam.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
            smoothedRotatedMovement = cam.transform.TransformDirection(movement); //Movement rotated with the camera
            plainRotatedMovement = cam.transform.TransformDirection(plainMovement); //Only used for debug
            cam.transform.rotation = temporaryCameraRotation;

            if (sharpTurn) transform.rotation = Quaternion.LookRotation(smoothedRotatedMovement); //Instant turn
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(smoothedRotatedMovement), Time.deltaTime * rotationSpeed); //Smooth turn
            forwardSpeed = Mathf.Clamp01(smoothedRotatedMovement.magnitude);
            wasMovingLastFrame = true;
            hasMoved = true;
        }

        //Keep track of previous frame movement variables
        previousMovement = movement;
        previousPlainMovement = plainMovement;

        runVariableSmoothed = Mathf.Clamp01(runVariableSmoothed + ((isRunning ? timeToRunMultiplier : -timeToRunMultiplier) * Time.deltaTime)); //Gradually start running

        animator.SetFloat("Forward", forwardSpeed);
        animator.SetFloat("Running", runVariableSmoothed);
    }

    private Vector3 FPSLookAt;
    private void FPS() {
        float horizontal = canMove? Input.GetAxis("Horizontal") : 0;
        float vertical = canMove?Input.GetAxis("Vertical") : 0;

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        Quaternion temporaryCameraRotation = cam.transform.rotation;
        cam.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        movement = cam.transform.transform.TransformDirection(movement); //Movement rotated with the camera
        cam.transform.rotation = temporaryCameraRotation;

        animator.SetFloat("Forward", movement.sqrMagnitude > 0f ? 1f : 0f);
        FPSLookAt = movement;
        movement.y -= gravity;
        controller.Move(movement * fpsSpeed * Time.deltaTime);
    }
}
