using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance;

    private Animator _animator;
    private bool _facingRight;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetDashRating(NoteTracker.HitInfo hitInfo, bool facingRight)
    {
        _facingRight = facingRight;
        _animator.SetInteger("Dash Rating", ((int)hitInfo.rating));
        _animator.SetBool("Facing Right", facingRight);
        _animator.SetTrigger("Dash");
    }

    public void PlayWallBump()
    {
        _animator.SetTrigger("Wall Bump");
        _animator.SetBool("Facing Right", _facingRight);
    }

    public void PlayRespawn(bool spawnRight)
    {
        _facingRight = spawnRight;

        _animator.SetTrigger("Respawn");
        _animator.SetBool("Facing Right", spawnRight);
    }

    void FinishRespawnClip()
    {
        RespawnManager.Instance.isRespawning = false;
    }
}