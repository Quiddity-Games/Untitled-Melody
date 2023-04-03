using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// The script responsible for controlling the player. (Named "alt" to differentiate it from a previous version of the player controller that used to be in the project.)
/// </summary>
public class AltPlayerController : MonoBehaviour
{
    public Transform cursorTransform;   //Used to check the location of the player's cursor
    private Vector2 dashDirection;  //Used to determine the direction in which the player will dash, based on where their cursor is
    bool isDashing;

    float dashForce;    //The force applied to the player when they dash
    public float dashForceMultiplier;   //A coefficient value used when determining the force of each dash
    public float dashingTime;
    public float maxDashDistanceMultiplier; //Used to limit the distance of the dash to a certain max radius

    public GameObject gameCamera;

    public GameObject cursorAfterImage; //A gameObject that briefly appears at the location where the player last clicked/tapped (or at the maximum distance away their dash can take them, if their click location was beyond that radius) so that they can compare the location of their click/tap with their resulting movement

    // Update is called once per frame
    void Update()
    {        
        //Moves in-game cursor to the location of the player's computer cursor
        cursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

        //Changes the cursor's color if the player hasn't yet "spent" their current dash attempt for the current beat
        if(BeatTracker.instance.canDash)
        {
            cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else //Changes the cursor's color if the player has spent their dash for this beat
        {
            cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
        // Determine if the player wants to dash, and hasn't spent their dash yet
        if(Input.GetKeyDown(KeyCode.Mouse0) && BeatTracker.instance.canDash)
        {
            BeatTracker.instance.canDash = false;   //Player's click is "spent" until the next beat
            StartCoroutine(ForceDash());
        }

    }

    /// <summary>
    /// Applies a force to the player character that pushes them in the direction of the cursor.
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
            //Restricts the dash distance to the max radius when necessary, to ensure the player can't dash beyond a certain distance
            dashForce = dashForceMultiplier * maxDashDistanceMultiplier;
        }

        //Applies a force, but only if the player is "on beat"
        if(BeatTracker.instance.onBeat)
        {
            isDashing = true;

            //Drops an "afterimage" of the cursor wherever the player just clicked
            GameObject lastClickLocation = Instantiate(cursorAfterImage);

            //Changes the location of this afterimage so that it's within the player's "max dash radius," if needed
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
            
            isDashing = false;
        }

        //Resets player's trail color
        gameObject.GetComponent<TrailRenderer>().startColor = Color.green;
        gameObject.GetComponent<TrailRenderer>().endColor = Color.green;
    }

    /// <summary>
    /// Causes the gameObject showing where the player last clicked/tapped to fade away.
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
}
