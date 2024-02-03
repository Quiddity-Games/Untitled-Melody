
using System;
using UnityEngine;
using DG.Tweening;

public class DashTracker : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerControl _playerControl;

    [SerializeField] private AudioSource _dashSource;
    [SerializeField] private AudioSource _wallBumpSource;
    [SerializeField] private NoteTracker _noteTracker;
    [SerializeField] private PlayerAnimationController _animationControl;
    [SerializeField] private ClickManager _clickManager;

    [Serializable]
    private struct DashSounds
    {
        public AudioClip badSound;
        public AudioClip goodSound;
        public AudioClip greatSound;
        public AudioClip perfectSound;
    }

    [SerializeField] private DashSounds sounds;
    [SerializeField] private AudioClip wallBumpSound;

    private void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Enable();
    }

    private void Awake()
    {
        //_noteTracker.HitCallback += info => {MoveTracker();};
        _noteTracker.HitCallback += PlaySound;
    }

    private void OnDestroy()
    {
        //_noteTracker.HitCallback -= info => { MoveTracker(); };
        _noteTracker.HitCallback -= PlaySound;
    }

    /*private void MoveTracker()
    {
        Vector2 mousePos = _playerControl.Dreamworld.MousePosition.ReadValue<Vector2>();
        Vector2 dashLocation =
            Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
        this.transform.position = dashLocation;
    }*/

    private void PlaySound(NoteTracker.HitInfo hitInfo)
    {
        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                _dashSource.PlayOneShot(sounds.badSound);
                break;
            case NoteTracker.BeatRating.GOOD:
                _dashSource.PlayOneShot(sounds.goodSound);
                break;
            case NoteTracker.BeatRating.GREAT:
                _dashSource.PlayOneShot(sounds.greatSound);
                break;
            case NoteTracker.BeatRating.PERFECT:
                _dashSource.PlayOneShot(sounds.perfectSound);
                break;
            default:
                _dashSource.PlayOneShot(sounds.badSound);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            _wallBumpSource.PlayOneShot(wallBumpSound);
            _animationControl.PlayWallBump();
        }
    }
}
