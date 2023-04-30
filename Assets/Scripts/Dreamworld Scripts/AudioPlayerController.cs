using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{

    [SerializeField] private FloatVariable volume;
    [SerializeField] private AudioSource songPlayer;
    // Start is called before the first frame update
    void Start()
    {
        songPlayer.volume = .5f;
        volume.OnValueChange += UpdateVolume;
    }

    public void UpdateVolume()
    {
        songPlayer.volume = Mathf.Clamp(volume.Value, 0, 1);
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
