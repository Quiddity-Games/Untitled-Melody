using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    public Checkpoint currentCheckpoint;
    public bool spawnFacingRight;
    public bool isRespawning;
    
    private GameObject _player;

    [SerializeField] private AudioSource onDeathSound;

    private void Awake()
    {
        Instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        DreamworldEventManager.OnDeath += RespawnPlayer;
        DreamworldEventManager.OnDeath += onDeathSound.Play;
    }

    private void OnDisable()
    {
        DreamworldEventManager.OnDeath -= RespawnPlayer;
        DreamworldEventManager.OnDeath -= onDeathSound.Play;
    }

    public void Start()
    {
        
    }

    public void RespawnPlayer()
    {
        isRespawning = true;
        _player.transform.position = currentCheckpoint.transform.position; //Respawns the player at their most recent checkpoint
        _player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        PlayerAnimationController.Instance.PlayRespawn(spawnFacingRight);
    }
}
