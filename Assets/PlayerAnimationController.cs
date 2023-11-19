using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private bool _facingRight;

    private static readonly int IdleR = Animator.StringToHash("Idle (Right)");
    private static readonly int IdleL = Animator.StringToHash("Idle (Left)");
    private static readonly int DashMissR = Animator.StringToHash("Dash - Miss (Right)");
    private static readonly int DashMissL = Animator.StringToHash("Dash - Miss (Left)");
    private static readonly int DashGreatR = Animator.StringToHash("Dash - Great (Right)");
    private static readonly int DashGreatL = Animator.StringToHash("Dash - Great (Left)");
    private static readonly int DashPerfectR = Animator.StringToHash("Dash - Perfect (Right)");
    private static readonly int DashPerfectL = Animator.StringToHash("Dash - Perfect (Left)");
    private static readonly int BonkR = Animator.StringToHash("Bonk (Right)");
    private static readonly int BonkL = Animator.StringToHash("Bonk (Left)");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetDashRating(NoteTracker.HitInfo hitInfo, bool facingRight)
    {
        _facingRight = facingRight;
        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                if (facingRight)
                    _animator.CrossFade(DashMissR, 0, 0);
                else
                    _animator.CrossFade(DashMissL, 0, 0);
                break;
            case NoteTracker.BeatRating.GOOD:
                if (facingRight)
                    _animator.CrossFade(DashGreatR, 0, 0);
                else
                    _animator.CrossFade(DashGreatL, 0, 0);
                break;
            case NoteTracker.BeatRating.GREAT:
                if (facingRight)
                    _animator.CrossFade(DashGreatR, 0, 0);
                else
                    _animator.CrossFade(DashGreatL, 0, 0);
                break;
            case NoteTracker.BeatRating.PERFECT:
                if (facingRight)
                    _animator.CrossFade(DashPerfectR, 0, 0);
                else
                    _animator.CrossFade(DashPerfectL, 0, 0);
                break;
        }

        StartCoroutine(BackToIdle());
    }

    public void PlayWallBump()
    {
        if (_facingRight)
            _animator.CrossFade(BonkR, 0, 0);
        else
            _animator.CrossFade(BonkL, 0, 0);

        StartCoroutine(BackToIdle());
    }

    IEnumerator BackToIdle()
    {
        float animDuration = _animator.GetCurrentAnimatorClipInfo(0).Length;

        yield return new WaitForSeconds(animDuration);
        if (_facingRight)
            _animator.CrossFade(IdleR, 0, 0);
        else
            _animator.CrossFade(IdleL, 0, 0);
    }
}