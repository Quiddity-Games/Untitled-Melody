using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewController : MonoBehaviour
{
    bool isGrounded;
    [SerializeField] Rigidbody2D rb;
    private LayerMask worldObjects = 1 << 0;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, worldObjects);


        // Determine dash inputs
        Dash();

        //Reset DashCount (current) on grounded
        if (dashCount < maxDashes && isGrounded)
        {
            //set your current alloted dashes to your maximum dashes
            dashCount = maxDashes;
        }

        // Apply the dash force
        if (dashCount > 0)
            ApplyDash();
    }

    void Dash()
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
    }

    /// <summary>
    /// Apply the force for the dash to the player
    /// </summary>
    void ApplyDash()
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
    }
}
