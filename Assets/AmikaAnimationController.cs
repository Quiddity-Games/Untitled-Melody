using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmikaAnimationController : MonoBehaviour
{
    [SerializeField] private bool _facingRight;
    [SerializeField] private BoolVariable _isFacingRight;
    [SerializeField] private NoteTracker.BeatRating _beatRating;

    [Space(10)]
    [SerializeField] private NoteTracker _noteTracker;

    private Animator _animator;

    private static readonly int IdleR = Animator.StringToHash("Idle (Right)");
    private static readonly int IdleL = Animator.StringToHash("Idle (Left)");
    private static readonly int DashMissR = Animator.StringToHash("Dash - Miss (Right)");
    private static readonly int DashMissL = Animator.StringToHash("Dash - Miss (Left)");
    private static readonly int DashGreatR = Animator.StringToHash("Dash - Great (Right)");
    private static readonly int DashGreatL = Animator.StringToHash("Dash - Great (Left)");

    private void OnEnable()
    {
        _noteTracker.HitCallback += SetDashRating;
    }

    private void OnDestroy()
    {
        _noteTracker.HitCallback -= SetDashRating;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isFacingRight.Value = true;
    }

    void PlayAnimation(int animHashR, int animHashL)
    {
        if (_isFacingRight.Value)
            _animator.CrossFade(animHashR, 0, 0);
        else
            _animator.CrossFade(animHashL, 0, 0);
    }

    void SetDashRating(NoteTracker.HitInfo hitInfo)
    {
        _facingRight = _isFacingRight.Value;

        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                PlayAnimation(IdleR, DashMissL);
                break;
            case NoteTracker.BeatRating.GOOD:
                PlayAnimation(DashGreatR, DashGreatL);
                break;
            case NoteTracker.BeatRating.GREAT:
                PlayAnimation(DashGreatR, DashGreatL);
                break;
            case NoteTracker.BeatRating.PERFECT:
                PlayAnimation(IdleR, IdleL);
                break;
        }
    }
}