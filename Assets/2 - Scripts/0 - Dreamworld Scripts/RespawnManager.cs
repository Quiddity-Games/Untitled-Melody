using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    [SerializeField] private CheckpointSignal checkpoint;

    public Checkpoint currentCheckpoint;
    public bool spawnFacingRight;
    public bool isRespawning;
    
    private GameObject _player;

    private void Awake()
    {
        Instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RespawnPlayer()
    {
        isRespawning = true;
        _player.transform.position = currentCheckpoint.transform.position; //Respawns the player at their most recent checkpoint
        _player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        PlayerAnimationController.Instance.PlayRespawn(spawnFacingRight);
    }
}
