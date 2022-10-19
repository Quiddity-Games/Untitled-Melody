using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltPlayerController : MonoBehaviour
{
    public Transform cursorTransform;
    private Vector2 dashDirection;
    public float dashForce;
    public float dashingTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Moves in-game cursor to the location of the player's computer cursor
        cursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

        //Changes the cursor's color on-beat
        if(BeatTracker.instance.onBeat)
        {
            cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            cursorTransform.localScale += Vector3.one * .005f / 2;
            //beatCursor.transform.localScale += Vector3.one * .005f / 4;
        }
        else
        {
            cursorTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            cursorTransform.localScale = Vector3.one;
            // beatCursor.transform.localScale = Vector3.one;
        }

        // Determine if the player wants to dash
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(ForceDash());
        }
    }

    /// <summary>
    /// Applies a force to the player character that pushes them in the direction of the cursor. 
    /// Different from dashes in previous versions of the game in that it uses forces rather than velocities.
    /// </summary>
    private IEnumerator ForceDash()
    {
        //Determines the dash's direction by calculating the mouse's position relative to the player
        dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dashDirection = dashDirection.normalized;

        //Apply Force
        if(BeatTracker.instance.onBeat)
        {
            //Temporarily cuts gravity to prevent dash from being "softened" by gravity pulling you downward
            float gravity = this.GetComponent<Rigidbody2D>().gravityScale;
            this.GetComponent<Rigidbody2D>().gravityScale = 0f;

            //Applies dash
            this.GetComponent<Rigidbody2D>().AddForce(dashDirection * dashForce);

            //Changes player's trail color on dash
            gameObject.GetComponent<TrailRenderer>().startColor = Color.yellow;
            gameObject.GetComponent<TrailRenderer>().endColor = Color.yellow;

            //Pause while the dash is underway
            yield return new WaitForSeconds(dashingTime);

            //Resets gravity to full when dash is done
            this.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }

        //Resets player's trail color
        gameObject.GetComponent<TrailRenderer>().startColor = Color.green;
        gameObject.GetComponent<TrailRenderer>().endColor = Color.green;
    }
}
