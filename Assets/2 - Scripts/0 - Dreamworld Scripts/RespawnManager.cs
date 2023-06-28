using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private CheckpointSignal checkpoint;

    private Vector3 currentCheckpointPosition;
    
    private GameObject _player;

    private void Awake()
    {
        checkpoint.OnCheckpointEnter += UpdateCheckpointPosition;
        _player = GameObject.FindGameObjectWithTag("Player");

    }

    public void UpdateCheckpointPosition(Vector3 position)
    {
        currentCheckpointPosition = position;
    }

    public void RespawnPlayer()
    {
        _player.transform.position = new Vector2(Checkpoint.currentCheckpoint.transform.position.x, Checkpoint.currentCheckpoint.transform.position.y);    //Respawns the player at their most recent checkpoint
        _player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
