using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

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
    public bool shouldDash;
    private float dashScalar;
    public float dashMod;    
    private float timeSinceGameStart;
    public int maxDashes;
    public int dashCount = 1;

    //New Dash
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    float originalGravity;

    private void Start()
    {
        instance = this;
        originalGravity = rb.gravityScale;
    }

    /// <summary>
    /// Determine the player's state and inputs once per frame
    /// </summary>
    void Update()
    {
        if (isGrounded)
        {
            rb.gravityScale = 0f;
        } else if (canDash)
        {
            rb.gravityScale = originalGravity;
        }
            // Check horizontal input
        horizontalInput = GetInput().x;


        // Check jump input
        if (!isDashing)
        {
            if (Physics2D.Raycast(transform.position - new Vector3(gameObject.transform.localScale.x / 2, 0, 0), Vector2.down, 0.6f, worldObjects) ||
             (Physics2D.Raycast(transform.position + new Vector3(gameObject.transform.localScale.x / 2, 0, 0), Vector2.down, 0.6f, worldObjects)))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
           
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }

        // Wall slide inputs
        WallSlide();
        WallJump();

        // Determine dash inputs
        DashCheck();

        //Reset DashCount (current) on grounded
        if (dashCount < maxDashes && (isGrounded || isTouchingWall))
        {
            //set your current alloted dashes to your maximum dashes
            dashCount = maxDashes;
        }
    }

    /// <summary>
    /// Apply physics to the player depending on inputs determined in the Update function
    /// </summary>
    private void FixedUpdate()
    {
        if (!isDashing)
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
            //EnhanceJump();

            // Slow player's vertical velocity on wall slides
            SlowWallSlide();

            // Launch the player off of a wall
            WallJumpLaunch();
        }

        // Apply the dash force
       // if (dashCount > 0)
          //  ApplyDash();
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
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right, 0.5125f, worldObjects);

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
            isTouchingWall = Physics2D.Raycast(transform.position, Vector2.left, 0.5125f, worldObjects);

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
            rb.AddForce(new Vector2(xWallForce * wallDirectionScalar, yWallForce), ForceMode2D.Impulse);
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
    /*void Dash()
    {
        
        // Calculate the time since start timer
        timeSinceGameStart += Time.deltaTime;

        // Determine if the player wants to dash
        if (Input.GetKeyDown(KeyCode.Mouse0) && !shouldDash && dashCount > 0)
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
    } */

    /// <summary>
    /// Apply the force for the dash to the player
    /// </summary>
   /* void ApplyDash()
    {
       
        if (shouldDash)
        {

            if (BeatTracker.instance.onBeat)
            {
                dashMod = 2;
            }
            else
            {
                dashMod = 1;
            }
            rb.AddForce(dashDirection * dashForce * dashMod, ForceMode2D.Impulse);
            dashCount--;
            shouldDash = false;
        }
    }*/

    public void DashCheck()
    {
        //change cursor color on beat
        if (dashCount > 0)
        {
            if (BeatTracker.instance.onBeat)
            {
                cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                cursorTransform.localScale += Vector3.one * .005f/2;
            }
            else
            {
                cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                cursorTransform.localScale = Vector3.one;
            }
        } else
        {
            cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            cursorTransform.localScale = Vector3.one;
        }
        // Determine if the player wants to dash
        if (Input.GetKeyDown(KeyCode.Mouse0) && !shouldDash && dashCount > 0)
        {
            dashCount--;
            StartCoroutine(Dash());
        }
        // Track the cursor and give a visual representation of the timings
        cursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        groundLinearDrag = 2.5f;
        airLinearDrag = 2.5f;
        rb.drag = 2.5f;
        //Dash Direction
        dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dashDirection = dashDirection.normalized;
        //Apply Force
        if (BeatTracker.instance.onBeat)
        {
            rb.velocity = new Vector2(dashDirection.x * dashingPower, dashDirection.y * dashingPower);
        } else
        {
            rb.velocity = new Vector2((dashDirection.x * dashingPower)/1.75f, (dashDirection.y * dashingPower)/1.75f);
        }
        yield return new WaitForSeconds(dashingTime);
        rb.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(.2f);
        rb.gravityScale = originalGravity;
        groundLinearDrag = 10;
        airLinearDrag = 2.5f;
        //yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
