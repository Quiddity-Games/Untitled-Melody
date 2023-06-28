using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the behavior of -- and attached to -- checkpoints.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    public static GameObject currentCheckpoint; //Tracks the player's most recent checkpoint
    ParticleSystem checkPointBurst; //Used to emit a pulse when the player touches a checkpoint

    [SerializeField] private CheckpointSignal _checkpointSignal;

    private bool _used;
    
    // Start is called before the first frame update
    void Start()
    {
        _used = false; 
        checkPointBurst = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Checks to see if the player has collided with the checkpoint.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_used)
        {
            currentCheckpoint = gameObject; //Sets this checkpoint as the player's most recent checkpoint
            checkPointBurst.Emit(20);
            _used = true;
            _checkpointSignal.OnCheckpointEnter(transform.position);
            //"Saves" the progress the player has made in acquiring collectables up until this checkpoint

            //GameManager.instance.tempScore = 0; //Scoring/points funtionality removed for now; otherwise, resets the player's "temporary score," meaning they won't lose these points the next time they die
        }
    }
}
