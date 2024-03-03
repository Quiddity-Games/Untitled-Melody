using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class HazardBase : MonoBehaviour
{
    protected bool canMove;

    public virtual void OnKill()
    {
        
    }
    
    /// <summary>
    /// Checks if the Hazard has collided with the player.
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.DEATH);
        }
    }
}
