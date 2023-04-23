using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script that controls the behavior of -- and is attsched to -- the central gameObject of the piranha hazard object. 
/// </summary>
public class PiranhaCore : MonoBehaviour
{
    public float speed; //Speed at which the piranha can move
    Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Used to move the piranha towards the player.
    /// </summary>
    /// <param name="destination"></param>
    public void MovePiranhaCore(Vector3 destination)
    {
        float xDistance = destination.x - transform.position.x;
        float yDistance = destination.y - transform.position.y;
        float hypotenuseDistance = Vector2.Distance(new Vector2 (destination.x, destination.y), new Vector2(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y));

        float velocityXComponent = Mathf.Cos(Mathf.Acos(xDistance / hypotenuseDistance));
        float velocityYComponent = Mathf.Sin(Mathf.Asin(yDistance / hypotenuseDistance));

        //Blocks the piranha from moving for one frame if the player is respawning on that frame (prevents the piranha from drifting in the player's direction after they have just killed them)
        if(GameManager.instance.stopHazardsMove == false)
        {
            _rigidbody2D.velocity = new Vector2(velocityXComponent * speed * Time.deltaTime, velocityYComponent * speed * Time.deltaTime);
        } else
        {
            GameManager.instance.stopHazardsMove = false;
        }
    }

    /// <summary>
    /// Checks if the piranha has collided with the player.
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            GameManager.instance.OnDeath(this.gameObject.name);
        }
    }
}
