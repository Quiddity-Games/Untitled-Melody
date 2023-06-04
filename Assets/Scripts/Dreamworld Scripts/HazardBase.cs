using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class HazardBase : MonoBehaviour
{


    

    [SerializeField] private GameEvent OnCollision;
    protected bool canMove;
    // Start is called before the first frame update

   

 
    /// <summary>
    /// Checks if the Hazard has collided with the player.
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COLLISION");

        if(col.gameObject.CompareTag("Player"))
        {
            OnCollision.Raise();
        }
    }
}
