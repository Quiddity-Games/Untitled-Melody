using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] Rigidbody2D rb;


    [Header("Horizontal Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float groundLinearDrag;
    [SerializeField] float maxMoveSpeed;
    private float horizontalInput;
    private bool changingDirection => (rb.velocity.x > 0 && horizontalInput < 0) || (rb.velocity.x < 0 && horizontalInput > 0);


    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float airLinearDrag;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    private bool isGrounded;
    private bool jumpRequest;
    private LayerMask worldObjects = 1 << 0;


    [Header("Wall Slide")]
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] float wallSlidingSpeed;
    private bool isTouchingWall;
    private bool wallSlidingLeft;
    private bool wallSlidingRight;
    private int wallDirectionScalar;


    [Header("Wall Jump")]
    [SerializeField] float xWallForce;
    [SerializeField] float yWallForce;
    [SerializeField] float wallJumpTime;
    private bool wallJumping;

    [Header("Dash")]
    [SerializeField] Transform cursorTransform;
    [SerializeField] float dashForce;
    private Vector2 dashDirection;
    private bool shouldDash;
    private float dashScalar;
    private float timeSinceGameStart;

    /// <summary>
    /// Determine the player's state and inputs once per frame
    /// </summary>
    void Update()
    {
        // Check horizontal input
        horizontalInput = GetInput().x;

        // Check jump input
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, worldObjects);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }

        // Wall slide inputs
        WallSlide();
        WallJump();

        // Determine dash inputs
        Dash();
    }

    /// <summary>
    /// Apply physics to the player depending on inputs determined in the Update function
    /// </summary>
    private void FixedUpdate()
    {
        // Horizontal Movement physics
        HorizontalMovement();

        // Linear Drag
        if (isGrounded)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
        }

        // Handle jumping physics
        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }
        EnhanceJump();

        // Slow player's vertical velocity on wall slides
        SlowWallSlide();

        // Launch the player off of a wall
        WallJumpLaunch();

        // Apply the dash force
        ApplyDash();
    }

    /// <summary>
    /// Get the player's input
    /// </summary>
    /// <returns>Returns a vector 2 of the player input</returns>
    Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    /// <summary>
    /// Modify the player's horizontal velocity depending on key inputs
    /// </summary>
    void HorizontalMovement()
    {

        rb.AddForce(new Vector2(horizontalInput, 0f) * acceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
        }
    }

    /// <summary>
    /// Apply linear drag to the player when they are on the ground
    /// </summary>
    void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(horizontalInput) < 0.4f || changingDirection)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    /// <summary>
    /// Apply linear drag to the player when they are in the air
    /// </summary>
    void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    /// <summary>
    /// Apply a vertical force to the player if they are on the ground
    /// </summary>
    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Enhances the feel of the jump by changing the gravity scalar
    /// </summary>
    void EnhanceJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    /// <summary>
    /// Determine if the player is sliding down a wall
    /// </summary>
    void WallSlide()
    {
        // Determine if the player is wall sliding to the right
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, worldObjects);

        if (isTouchingWall && !isGrounded && rb.velocity.y <= 0)
        {
            wallSlidingRight = true;
            wallDirectionScalar = -1;
        }
        else
        {
            wallSlidingRight = false;
        }

        // Determine if the player is wall sliding to the left
        if (!isTouchingWall)
        {
            isTouchingWall = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, worldObjects);

            if (isTouchingWall && !isGrounded && rb.velocity.y <= 0)
            {
                wallSlidingLeft = true;
                wallDirectionScalar = 1;
            }
            else
            {
                wallSlidingLeft = false;
            }
        }
    }

    /// <summary>
    /// Slow the player's vertical velocity if they are sliding down a wall
    /// </summary>
    void SlowWallSlide()
    {
        // Slow the player down
        if (wallSlidingRight)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -Mathf.Infinity, 0), -wallSlidingSpeed);
        }
        else if (wallSlidingLeft)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 0, Mathf.Infinity), -wallSlidingSpeed);
        }
    }

    /// <summary>
    /// Determine if the player wants to jump off of a wall
    /// </summary>
    void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (wallSlidingLeft || wallSlidingRight))
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
    }

    /// <summary>
    /// Apply a force in the horizontal and vertical directions if the player wants to wall jump
    /// </summary>
    void WallJumpLaunch()
    {
        if (wallJumping)
        {
            rb.AddForce(new Vector2(xWallForce * wallDirectionScalar, yWallForce));
        }
    }

    /// <summary>
    /// Helper function called in WallJump(), changes a singular boolean value after the jump time
    /// </summary>
    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    /// <summary>
    /// Determine if the player should dash
    /// </summary>
    void Dash()
    {
        // Calculate the time since start timer
        timeSinceGameStart += Time.deltaTime;

        // Determine if the player wants to dash
        if (Input.GetKeyDown(KeyCode.Mouse0) && !shouldDash)
        {
            shouldDash = true;

            // Calculate the dash direction
            dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dashDirection = dashDirection.normalized;
        }

        // Calculate the dash scalar
        dashScalar = Mathf.Pow(Mathf.Sin(timeSinceGameStart * 0.75f * Mathf.PI), 2);

        // Track the cursor and give a visual representation of the timings
        cursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
        cursorTransform.localScale = Vector3.one * dashScalar;
    }

    /// <summary>
    /// Apply the force for the dash to the player
    /// </summary>
    void ApplyDash()
    {
        if (shouldDash)
        {
            rb.AddForce(dashDirection * dashForce * dashScalar, ForceMode2D.Impulse);
            shouldDash = false;
        }
    }
}
