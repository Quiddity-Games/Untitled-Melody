using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DashSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _dashSource;
    [SerializeField] private AudioSource _bonkSource;
    [SerializeField] private NoteTracker _noteTracker;
    
    [Serializable]
    private struct DashSounds
    {
        public AudioClip badSound;
        public AudioClip goodSound;
        public AudioClip greatSound;
        public AudioClip perfectSound;
    }


    [SerializeField] private DashSounds sounds;
    [SerializeField] private AudioClip bonkSound;

    private void Start()
    {
        _noteTracker.HitCallback += PlaySound;
    }

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
            _bonkSource.PlayOneShot(bonkSound);
        }
    }
}
