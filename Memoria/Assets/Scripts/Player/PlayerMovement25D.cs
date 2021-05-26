using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement25D : MonoBehaviour {
    //[Header("Movement")]
    //[SerializeField] public float rotationSpeed = 5.0f;
    //[SerializeField] public float timeToRunMultiplier = 1.0f;
    //[SerializeField] public float movementDragMultiplier = 0.85f;
    //[SerializeField] public float movementDecreaseWhenNotMoving = 0.01f;
    //[SerializeField] public float startMovingTreshold = 0.25f;
    //
    //private Transform cam;
    private Animator animator;
    //
    //private Vector3 smoothedRotatedMovement = Vector3.zero;
    //private Vector3 previousPlainMovement = Vector3.zero;
    //private Vector3 plainRotatedMovement = Vector3.zero;
    //private Vector3 previousMovement = Vector3.zero;
    //private float runVariableSmoothed = 0.0f;
    //private float forwardSpeed = 0.0f;
    //private bool wasMovingLastFrame;
    //
    //private CharacterController controller;
    //public float fpsSpeed = 2.5f;
    //private float gravity = 9.81f;
    //
    //[HideInInspector]
    //public bool inHouse;

    //public bool hasMoved = false;
    //public bool canMove = false;
    
    public float moveSpeed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;
    public LayerMask platformLayerMask;
    private Collider2D playerCollider;

    private bool isGrounded;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 4f;

    public int extraJumpsValue;

    public float hangTime = 0.2f;
    private float hangCounter;

    public float jumpBufferLength;
    private float jumpBufferCount;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        //cam = Camera.main.transform;
        animator = GetComponent<Animator>();
        //controller = GetComponent<CharacterController>();
        //inHouse = false;
    }

    void Update() {
        Jump();
        moveInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.IsTouchingLayers(playerCollider, platformLayerMask);
        MoveHorizontal();
    }

    private void MoveHorizontal() {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetAxis("Horizontal") < 0) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (Input.GetAxis("Horizontal") > 0) {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        animator.SetFloat("Forward", rb.velocity.magnitude);
    }
    private void Jump() {
        //to manage hang time
        if (isGrounded) {
            hangCounter = hangTime;
        } else {
            hangCounter -= Time.deltaTime;
        }

        //manage jump buffer
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpBufferCount = jumpBufferLength;
        } else {
            jumpBufferCount -= Time.deltaTime;
        }

        //checks hangtime and jumpbuffer
        if (jumpBufferCount >= 0 && hangCounter > 0f) {
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferCount = 0;
        }

        //checks if player is falling down
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //checks if player is going up and jump button is released
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Adventure() {
        //bool isRunning = true;
        //float horizontal = canMove ? Input.GetAxis("Horizontal") : 0.0f;
        //float vertical = canMove ? Input.GetAxis("Vertical") : 0.0f;
        //float rawHorizontal = canMove ? Input.GetAxisRaw("Horizontal") : 0.0f;
        //float rawVertical = canMove ? Input.GetAxisRaw("Vertical") : 0.0f;
        //
        //Vector3 plainMovement = new Vector3(horizontal, 0, vertical); //This frame's movement store in a vector.
        //Vector3 plainRawMovement = new Vector3(rawHorizontal, 0, rawVertical); //This frame's movement store in a vector.
        //
        //Vector3 movement = previousMovement * movementDragMultiplier; //Keep some movement speed from the previous frame.
        //
        ////Check if we're making a 180 turn
        //float dot = Vector3.Dot(previousPlainMovement, plainMovement);
        //bool sharpTurn = dot < 0.1f;
        //if (sharpTurn) movement = Vector3.zero;
        //if (plainRawMovement == Vector3.zero) { //No keys are pressed so no movement input
        //    if (!sharpTurn) {
        //        if (wasMovingLastFrame) runVariableSmoothed = 0.0f; //Player just stopped pressing movement keys
        //        forwardSpeed = Mathf.Clamp01(forwardSpeed - movementDecreaseWhenNotMoving); //Prevent instant movement stop
        //    }
        //    wasMovingLastFrame = false;
        //} else if (sharpTurn || (Mathf.Abs(plainMovement.x) + Mathf.Abs(plainMovement.z)) > startMovingTreshold) {//If any keys are pressed and thus the player should be moving. Or if there is a sharp turn
        //    if (Input.GetButton("Sprint")) isRunning = false;
        //    movement += plainMovement; //Add input movement to movement resulting from the previous frame
        //
        //    Quaternion temporaryCameraRotation = cam.rotation;
        //    cam.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
        //    smoothedRotatedMovement = cam.TransformDirection(movement); //Movement rotated with the camera
        //    plainRotatedMovement = cam.TransformDirection(plainMovement); //Only used for debug
        //    cam.rotation = temporaryCameraRotation;
        //
        //    if (sharpTurn) transform.rotation = Quaternion.LookRotation(smoothedRotatedMovement); //Instant turn
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(smoothedRotatedMovement), Time.deltaTime * rotationSpeed); //Smooth turn
        //    forwardSpeed = Mathf.Clamp01(smoothedRotatedMovement.magnitude);
        //    wasMovingLastFrame = true;
        //    hasMoved = true;
        //}
        //
        ////Keep track of previous frame movement variables
        //previousMovement = movement;
        //previousPlainMovement = plainMovement;
        //
        //runVariableSmoothed = Mathf.Clamp01(runVariableSmoothed + ((isRunning ? timeToRunMultiplier : -timeToRunMultiplier) * Time.deltaTime)); //Gradually start running
        //
        //animator.SetFloat("Forward", forwardSpeed);
        //animator.SetFloat("Running", runVariableSmoothed);
    }

    private void FPS() {
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //
        //Vector3 movement = new Vector3(horizontal, 0, vertical);
        //
        //Quaternion temporaryCameraRotation = cam.rotation;
        //cam.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
        //movement = cam.TransformDirection(movement); //Movement rotated with the camera
        //cam.rotation = temporaryCameraRotation;
        //
        //movement.y -= gravity;
        //controller.Move(movement * fpsSpeed * Time.deltaTime);
    }
}
