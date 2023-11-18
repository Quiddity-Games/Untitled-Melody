using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmikaAnimationController : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private NoteTracker _noteTracker;

    private Animator _animator;

    private static readonly int IdleR = Animator.StringToHash("Idle (Right)");
    private static readonly int IdleL = Animator.StringToHash("Idle (Left)");
    private static readonly int DashMissR = Animator.StringToHash("Dash - Miss (Right)");
    private static readonly int DashMissL = Animator.StringToHash("Dash - Miss (Left)");
    private static readonly int DashGreatR = Animator.StringToHash("Dash - Great (Right)");
    private static readonly int DashGreatL = Animator.StringToHash("Dash - Great (Left)");
    private static readonly int DashPerfectR = Animator.StringToHash("Dash - Perfect (Right)");
    private static readonly int DashPerfectL = Animator.StringToHash("Dash - Perfect (Left)");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetDashRating(NoteTracker.HitInfo hitInfo, bool facingRight)
    {
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
    }
}