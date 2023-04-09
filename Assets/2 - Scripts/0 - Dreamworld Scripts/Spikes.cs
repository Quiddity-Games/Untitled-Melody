using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guides the behavior of the spikes hazard type, and is attached to Spikes gameObjects.
/// </summary>
public class Spikes : MonoBehaviour
{
    /// <summary>
    /// Checks if the player has collided with the spikes.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.OnDeath(this.gameObject.name); //Death functionality handled by the Game Manager
        }
    }
}
