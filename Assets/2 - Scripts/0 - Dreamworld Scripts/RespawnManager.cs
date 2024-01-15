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

    [SerializeField] private AudioSource onDeathSound;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Start()
    {
        DreamworldEventManager.Instance.RegisterVector3EventResponse(DreamworldVector3EventEnum.CHECKPOINT_POSITION, UpdateCheckpointPosition);
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.DEATH, RespawnPlayer);
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.DEATH, onDeathSound.Play);
    }

    public void UpdateCheckpointPosition(Vector3 position)
    {
        currentCheckpointPosition = position;
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
