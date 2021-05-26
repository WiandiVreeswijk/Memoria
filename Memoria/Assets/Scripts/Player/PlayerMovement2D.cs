using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        Jump();
        moveInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.IsTouchingLayers(playerCollider, platformLayerMask);
        MoveHorizontal();
    }

    private void MoveHorizontal()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void Jump()
    {
        //to manage hang time
        if (isGrounded)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        //manage jump buffer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        //checks hangtime and jumpbuffer
        if (jumpBufferCount >= 0 && hangCounter > 0f)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferCount = 0;
        }

        //checks if player is falling down
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //checks if player is going up and jump button is released
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
}
