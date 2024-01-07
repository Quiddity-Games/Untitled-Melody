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
        DreamworldEventManager.Instance.RegisterVoidEventResponse(
            DreamworldVoidEventEnum.GAME_START, Play);
        DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.ISPAUSED,
            TogglePause);
    }

    public void Play()
    {
        songPlayer.Play();
    }

    public void TogglePause(bool isPaused)
    {
        if (isPaused)
        {
            songPlayer.Pause();
        }
        else
        {
            songPlayer.UnPause();
        }
    }
}
