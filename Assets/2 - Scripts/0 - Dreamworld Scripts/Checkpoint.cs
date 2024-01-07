using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Responsible for the behavior of -- and attached to -- checkpoints.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    ParticleSystem checkPointBurst; //Used to emit a pulse when the player touches a checkpoint
    public bool spawnFacingRight;

    [SerializeField] private CheckpointSignal _checkpointSignal;
    [SerializeField] private Animator curtainAnimator;
    [SerializeField] private SpriteRenderer starsSprite;

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
            if (RespawnManager.Instance.currentCheckpoint != null)
                RespawnManager.Instance.currentCheckpoint.DisableStars();

            checkPointBurst.Emit(20);
            _used = true;

            CollectionScoreController.Instance.RecordCurrentCollection();

            RespawnManager.Instance.currentCheckpoint = this;
            RespawnManager.Instance.spawnFacingRight = spawnFacingRight;

            curtainAnimator.SetTrigger("Activated");
            DOTween.Sequence().InsertCallback(2f, () => curtainAnimator.gameObject.SetActive(false));
        }
    }

    /// <summary>
    /// Disable the stars animation when a new checkpoint overwrites the current one.
    /// </summary>
    public void DisableStars()
    {
        DOTween.Sequence().Append(starsSprite.DOFade(0, 0.6f)).AppendCallback(() => starsSprite.gameObject.SetActive(false));
    }
}
