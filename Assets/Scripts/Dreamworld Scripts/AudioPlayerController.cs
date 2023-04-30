using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{

    [SerializeField] private AudioSource songPlayer;
    // Start is called before the first frame update
    void Start()
    {
        songPlayer.volume = .5f;
        
    }

    public void Play()
    {
        songPlayer.Play();
    }

    public void Pause()
    {
        songPlayer.Pause();
    }
}
