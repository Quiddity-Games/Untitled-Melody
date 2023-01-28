using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class AltPlayerController : MonoBehaviour
{
    public Transform cursorTransform;
    private Vector2 dashDirection;
    bool isDashing;
    //bool canClick;  //Used to determine if the player has "spent" their one click (dash attempt) they have for each beat

    float dashForce;
    public float dashForceMultiplier;
    public float dashingTime;
    public float maxDashDistanceMultiplier; //Used to limit the distance of the dash to a certain max radius

    float horizontalInput;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float acceleration;
    [SerializeField] float maxMoveSpeed;

    public GameObject gameCamera;

    // Start is called before the first frame update
    void Start()
    {   
        //canClick = true;
    }

    // Update is called once per frame
    void Update()
    {        
        //Debug.Log("Cursor Distance is: " + Vector3.Distance(cursorTransform.position, this.GetComponent<Transform>().position).ToString()); //Used for testing what the maximum dash distance should be

        //Cut  keyboard/horizontal movement for now
        /*
        if(!isDashing)
        {

            // Check horizontal input
            horizontalInput = GetInput().x;

            // Horizontal Movement physics
            HorizontalMovement();
        }
        */

        //Moves in-game cursor to the location of the player's computer cursor
        cursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

        //Changes the cursor's color depending on the game state
        if(BeatTracker.instance.canClick)
        {
            //Changes the cursor's color on-beat
            if(BeatTracker.instance.onBeat)
            {
                //Temporarily disabling cursor from pulsing to the beat, so we can test using only the "beat bars"
                /*
                cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                cursorTransform.localScale += Vector3.one * .005f / 2;
                //beatCursor.transform.localScale += Vector3.one * .005f / 4;
                */
            }
            else
            {
                cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                cursorTransform.localScale = Vector3.one;
                // beatCursor.transform.localScale = Vector3.one;
            }
        }
        else
        {
            cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            cursorTransform.localScale = Vector3.one;
        }

        /*
        // Resets the player's click
        if(BeatTracker.instance.onBeat)
        {
            canClick = true;
        }
        */
        
        // Determine if the player wants to dash
        if(Input.GetKeyDown(KeyCode.Mouse0) && BeatTracker.instance.canClick)
        {
            BeatTracker.instance.canClick = false;   //Player's click is "spent" until the next beat
            StartCoroutine(ForceDash());
        }

    }

    /// <summary>
    /// Applies a force to the player character that pushes them in the direction of the cursor. 
    /// Different from dashes in previous versions of the game in that it uses forces rather than velocities.
    /// </summary>
    private IEnumerator ForceDash()
    {
        gameCamera.GetComponent<CameraFollow>().smoothSpeed = gameCamera.GetComponent<CameraFollow>().dashingSmoothSpeed;  //Reincreases the smooth speed when the player dashes

        //Determines the dash's direction by calculating the mouse's position relative to the player
        dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dashDirection = dashDirection.normalized;

        //Adjusts the player's dash distance based on how far away the cursor is from the player
        if(Vector3.Distance(cursorTransform.position, this.GetComponent<Transform>().position) <= maxDashDistanceMultiplier)
        {
            dashForce = dashForceMultiplier * Vector3.Distance(cursorTransform.position, this.GetComponent<Transform>().position);
        } else
        {
            //Restricts the dash distance to the max radius when necessary
            dashForce = dashForceMultiplier * maxDashDistanceMultiplier;
        }

        //Apply Force
        if(BeatTracker.instance.onBeat)
        {
            isDashing = true;

            //Drops an "afterimage" of the cursor wherever the player just clicked
            GameObject lastClickLocation = Instantiate(cursorTransform.gameObject);
            lastClickLocation.GetComponent<Transform>().localScale = new Vector3(5f, 5f, 0);

            //Changes where this afterimage appears, based on whether the player clicked within the max dash distance or not
            if(Vector3.Distance(cursorTransform.position, this.GetComponent<Transform>().position) <= maxDashDistanceMultiplier)
            {
                lastClickLocation.GetComponent<Transform>().position = cursorTransform.position;
            } else
            {
                float angleX = Mathf.Acos((cursorTransform.position.x - this.GetComponent<Transform>().position.x) / Vector3.Distance(cursorTransform.position, this.GetComponent<Transform>().position));
                float angleY = Mathf.Asin((cursorTransform.position.y - this.GetComponent<Transform>().position.y) / Vector3.Distance(cursorTransform.position, this.GetComponent<Transform>().position));
                float actualXDist = Mathf.Cos(angleX) * maxDashDistanceMultiplier;
                float actualYDist = Mathf.Sin(angleY) * maxDashDistanceMultiplier;

                lastClickLocation.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x + actualXDist, this.GetComponent<Transform>().position.y + actualYDist, this.GetComponent<Transform>().position.z);
            }

            //Afterimage begins to fade
            StartCoroutine(VanishClickAfterImage(lastClickLocation));

            //Temporarily cuts gravity to prevent dash from being "softened" by gravity pulling you downward
            float gravity = this.GetComponent<Rigidbody2D>().gravityScale;
            this.GetComponent<Rigidbody2D>().gravityScale = 0f;

            //Zeroes out the player's current velocity so that they can have more precise control over their dash direction
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

            //Applies dash
            this.GetComponent<Rigidbody2D>().AddForce(dashDirection * dashForce);

            //Changes player's trail color on dash
            gameObject.GetComponent<TrailRenderer>().startColor = Color.yellow;
            gameObject.GetComponent<TrailRenderer>().endColor = Color.yellow;

            //Pause while the dash is underway
            yield return new WaitForSeconds(dashingTime);

            //Resets gravity to full when dash is done
            this.GetComponent<Rigidbody2D>().gravityScale = gravity;

            isDashing = false;

        }

        //Resets player's trail color
        gameObject.GetComponent<TrailRenderer>().startColor = Color.green;
        gameObject.GetComponent<TrailRenderer>().endColor = Color.green;
    }

    /// <summary>
    /// Causes the gameObject showing where the player last clicked to fade away.
    /// </summary>
    /// <param name="click"></param>
    /// <returns></returns>
    private IEnumerator VanishClickAfterImage(GameObject click)
    {
        float alpha = click.GetComponent<SpriteRenderer>().color.a;

        while(alpha >= 0)
        {
            click.GetComponent<SpriteRenderer>().color = new Vector4(click.GetComponent<SpriteRenderer>().color.r, click.GetComponent<SpriteRenderer>().color.g, click.GetComponent<SpriteRenderer>().color.b, alpha);
            alpha -= 0.02f;

            yield return 0;
        }

        Destroy(click);

        yield return 0;
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

        if(Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
        }
    }
}
