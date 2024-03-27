using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHazardBase : HazardBase
{
    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] private Vector3 startingPosition;
    
    public virtual void Start()
    {
        startingPosition = transform.position;
        canMove = true;
    }

    public override void OnKill()
    {
        ToggleMovement(false);
        ResetPosition();
        ResetVelocity();
        ToggleMovement(true);
    }
    
    public void ToggleMovement(bool enableMovement)
     {
         canMove = enableMovement;
     }
    
     public void ResetVelocity()
     {
         rb.velocity = new Vector2(0,0);
     }

     public void ResetPosition()
     {
         transform.position = startingPosition;
     }
}
