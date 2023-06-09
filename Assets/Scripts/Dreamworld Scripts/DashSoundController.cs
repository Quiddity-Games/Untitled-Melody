using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class DashSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource soundControl;

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

    private void Start()
    {
        _noteTracker.HitCallback += PlaySound;
    }

    private void PlaySound(NoteTracker.HitInfo hitInfo)
    {
        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                break;
            case NoteTracker.BeatRating.BAD:
                soundControl.PlayOneShot(sounds.badSound);
                break;
            case NoteTracker.BeatRating.GOOD:
                soundControl.PlayOneShot(sounds.goodSound);
                break;
            case NoteTracker.BeatRating.GREAT:
                soundControl.PlayOneShot(sounds.greatSound);
                break;
            case NoteTracker.BeatRating.PERFECT:
                soundControl.PlayOneShot(sounds.perfectSound);
                break;
            default:
                soundControl.PlayOneShot(sounds.badSound);
                break;
        }
        
    }
}
