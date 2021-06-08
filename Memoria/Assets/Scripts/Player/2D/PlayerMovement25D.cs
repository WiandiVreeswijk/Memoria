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
    public Vector2 groundColliderCheckSize = new Vector2(0.2f, 0.2f);

    public int extraJumpsValue;

    public float hangTime = 0.2f;
    private float hangCounter;

    public float jumpBufferLength;
    private float jumpBufferCount;

    private bool stunned = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        //groundCheckCollider = GetComponentInChildren<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        Jump();
        moveInput = Input.GetAxis("Horizontal");
    }

    bool wasTouching = false;
    private void FixedUpdate() {
        //IsTouching is checked for this frame and the previous frame to prevent Elena from 'tripping' over adjecent colliders
        bool areFeetGrounded = Physics2D.OverlapBox(transform.position, groundColliderCheckSize, 0, platformLayerMask);
        bool isTouchingLayers = Physics2D.IsTouchingLayers(playerCollider, platformLayerMask);
        bool touching = wasTouching || isTouchingLayers;
        wasTouching = isTouchingLayers;

        bool isGroundedThisFrame = areFeetGrounded && touching;

        //justLanded = !isGrounded && isGroundedThisFrame;
        isGrounded = isGroundedThisFrame;

        float horizontal = moveInput;
        float animatorForwardSpeed = Mathf.Clamp01(Mathf.Abs(rb.velocity.x));
        if (!stunned) rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //TODO rotate 180 animation
        if (horizontal < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (horizontal > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

        animator.SetFloat("Forward", animatorForwardSpeed);
        animator.SetFloat("Running", animatorForwardSpeed);
    }

    private void Jump() {
        //Manage hang time
        if (isGrounded) hangCounter = hangTime;
        else hangCounter -= Time.deltaTime;

        //Manage jump buffer
        if (Input.GetKeyDown(KeyCode.Space)) jumpBufferCount = jumpBufferLength;
        else jumpBufferCount -= Time.deltaTime;

        //checks hangtime and jumpbuffer
        if (jumpBufferCount >= 0 && hangCounter > 0f) {
            if (jumpBufferCount == jumpBufferLength) {
                //Just jumped
            }
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferCount = 0;
        }

        //Probably temporary as we'll need jump/hover/landing animations
        animator.SetFloat("Jump", isGrounded ? 0 : 1);

        //Check if player is falling down
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Check if player is going up and jump button is released
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void Stunned(float duration) {
        stunned = true;
        if (stunned) gameObject.GetComponent<PlayerVisualEffects>().DoBlink(0.1f, 4);
        Utils.DelayedAction(duration, () => stunned = false);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(groundColliderCheckSize.x, groundColliderCheckSize.y, 0.02f));
    }
}
