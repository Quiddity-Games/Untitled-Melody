using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script that controls the behavior of -- and is attsched to -- the central gameObject of the piranha hazard object. 
/// </summary>
public class PiranhaCore : MovingHazardBase
{
    public float speed; //Speed at which the piranha can move

    
    /// <summary>
    /// Used to move the piranha towards the player.
    /// </summary>
    /// <param name="destination"></param>
    public void MovePiranhaCore(Vector3 destination)
    {
        //Blocks the piranha from moving for one frame if the player is respawning on that frame (prevents the piranha from drifting in the player's direction after they have just killed them)
        if (!canMove)
        {
            return;
        }
        float xDistance = destination.x - transform.position.x;
        float yDistance = destination.y - transform.position.y;
        float hypotenuseDistance = Vector2.Distance(new Vector2 (destination.x, destination.y), new Vector2(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y));

        float velocityXComponent = Mathf.Cos(Mathf.Acos(xDistance / hypotenuseDistance));
        float velocityYComponent = Mathf.Sin(Mathf.Asin(yDistance / hypotenuseDistance));
        
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(velocityXComponent * speed * Time.deltaTime,
            velocityYComponent * speed * Time.deltaTime), Time.deltaTime);
    }

   
}
