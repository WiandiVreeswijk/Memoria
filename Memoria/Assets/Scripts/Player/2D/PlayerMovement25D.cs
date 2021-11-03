using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.SceneTemplate;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement25D : MonoBehaviour {
    Tween stunnedTween = null;
    private Animator animator;

    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    public float extraJumpForce = 15f;
    private float moveInput;

    private Rigidbody2D rb;
    public LayerMask platformLayerMask;
    public LayerMask ignoreLandingSoundLayerMask;
    private Collider2D playerCollider;

    private bool isGrounded;
    public float fallMultiplier = 7f;
    public float lowJumpMultiplier = 5f;
    public Vector2 groundColliderCheckSize = new Vector2(0.55f, 0.2f);

    public bool extraJumpSupported = false;
    public float hangTime = 0.15f;
    private float hangCounter;

    public float jumpBufferLength = 0;
    private float jumpBufferCount;

    public float fallTimeout = 0.05f;
    private float fallTimeoutDelta;

    bool justLanded = false;
    private float previousYVelocity = 0.0f;

    bool canJump = false;
    bool canExtraJump = false;

    private bool stunned = false;

    public bool dashSupported = false;
    public bool canDash = false;
    private bool isDashing = false;

    public float dashDuration = 2.0f;
    private float dashDurationLeft = 0.0f;
    private float currentDashSpeed = 0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        fallTimeoutDelta = fallTimeout;
    }

    void Update() {
        Jump();
        Dash();
        moveInput = Input.GetAxis("Horizontal");
        moveInput += Hinput.anyGamepad.leftStick.horizontal;
        moveInput = Mathf.Clamp(moveInput, -1.0f, 1.0f);
    }

    private void FixedUpdate() {
        //IsTouching is checked for this frame and the previous frame to prevent Elena from 'tripping' over adjecent colliders
        bool areFeetGrounded = Physics2D.OverlapBox(transform.position, groundColliderCheckSize, 0, platformLayerMask);
        //bool isTouchingLayers = Physics2D.IsTouchingLayers(playerCollider, platformLayerMask);

        var hit = Physics2D.Raycast(transform.position, -Vector2.up, 10.0f, platformLayerMask);

        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = platformLayerMask;

        //bool touching = wasTouching || isTouchingLayers;
        // wasTouching = isTouchingLayers;
        bool isGroundedThisFrame = areFeetGrounded && rb.velocity.y <= 0.001f;
        bool ignoreLanding = hit.collider == null ? true : ignoreLandingSoundLayerMask.value == (ignoreLandingSoundLayerMask.value | (1 << hit.collider.gameObject.layer));

        justLanded = !isGrounded && isGroundedThisFrame;
        if (justLanded && !ignoreLanding) OnLand(previousYVelocity);
        isGrounded = isGroundedThisFrame;
        canJump = isGroundedThisFrame;
        bool jumpAnimation = false;
        if (ignoreLanding) {
            canJump = false;
            canExtraJump = false;
        }
        if (isGrounded) {
            isInExtraJump = false;
            fallTimeoutDelta = fallTimeout;
            if (!ignoreLanding) {
                //canJump = true;
                if (extraJumpSupported) canExtraJump = true;
            }
        }
        if (isGrounded || rb.velocity.y <= 0) isJumping = false;

        if (fallTimeoutDelta >= 0.0f) {
            fallTimeoutDelta -= Time.deltaTime;
        } else {
            jumpAnimation = true;
        }

        //Probably temporary as we'll need jump/hover/landing animations
        animator.SetFloat("Jump", jumpAnimation ? 1 : 0);

        float horizontal = moveInput;
        float animatorForwardSpeed = Mathf.Clamp01(Mathf.Abs(rb.velocity.x));
        if (!stunned) rb.velocity = new Vector2((moveInput * moveSpeed) + currentDashSpeed, isDashing ? 0 : rb.velocity.y);

        //TODO rotate 180 animation
        if (horizontal < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (horizontal > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

        animator.SetFloat("Forward", animatorForwardSpeed);
        animator.SetFloat("Running", animatorForwardSpeed);
        previousYVelocity = rb.velocity.y;
    }

    private void OnLand(float landingVelocity) {
        if (Mathf.Abs(landingVelocity) > 5.0f) {
            Globals.Player.VisualEffects25D.Land(rb.velocity.x);
            Globals.Player.PlayerSound.PlayJumpLandSound();
        }
    }

    private bool isInExtraJump = false;
    bool isJumping = false;
    private void Jump() {
        if (isDashing) return;
        //Manage hang time
        if (isGrounded) hangCounter = hangTime;
        else hangCounter -= Time.deltaTime;

        //Manage jump buffer
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1");
        bool jumpUp = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp("joystick button 1");
        if (jump && canJump && !stunned) {
            Globals.Player.PlayerSound.PlayJumpingSound();
            jumpBufferCount = jumpBufferLength;
            hangCounter = hangTime;
            canJump = false;
        } else if (jump && canExtraJump && !stunned) {
            Globals.Player.PlayerSound.PlayJumpingSound(); //Super jump sound?
            jumpBufferCount = jumpBufferLength;
            hangCounter = hangTime;
            isInExtraJump = true;
            canExtraJump = false;
        } else jumpBufferCount -= Time.deltaTime;

        //checks hangtime and jumpbuffer
        if (jumpBufferCount >= 0 && hangCounter > 0f) {
            if (jumpBufferCount == jumpBufferLength) {
                //Just jumped
                isJumping = true;
            }
            rb.velocity = Vector2.up * (isInExtraJump ? extraJumpForce : jumpForce);
            jumpBufferCount = 0;
        }

        //Check if player is falling down
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Check if player is going up and jump button is released
        else if (rb.velocity.y > 0 && (stunned || !isJumping || !jump)) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (!stunned && rb.velocity.y > 0 && isJumping && jumpUp) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Dash()
    {
        currentDashSpeed = 0f;
        if (!dashSupported) return;
        bool dash = Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown("joystick button 0");
        if (dash && !isDashing) {
            isDashing = true;
            dashDurationLeft = dashDuration;
        }

        if (isDashing) {
            Globals.Debugger.Print("a", "Dashing", 0.1f);
            currentDashSpeed = 15f;
            dashDurationLeft -= Time.deltaTime;
            if (dashDurationLeft <= 0) {
                isDashing = false;
                canDash = true;
            }
        }
    }

    public void Stunned(float duration) {
        stunned = true;
        if (stunned) gameObject.GetComponent<Player25DVisualEffects>().DoBlink(0.1f, 4);
        stunnedTween = Utils.DelayedAction(duration, () => stunned = false);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(groundColliderCheckSize.x, groundColliderCheckSize.y, 0.02f));
    }

    public void SetStunned(bool toggle, bool stopMovement, bool freeze) {
        stunnedTween?.Kill();
        stunned = toggle;
        if (stopMovement) rb.velocity = Vector2.zero;
        rb.isKinematic = freeze;
    }

    public Vector2 GetVelocity() {
        return rb.velocity;
    }
}
