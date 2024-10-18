using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{

    [SerializeField] private AudioSource songPlayer;

    private void OnEnable()
    {
        DreamworldEventManager.OnGameStart += songPlayer.Play;
        PauseManager.OnPaused += TogglePause;
        DreamworldEventManager.OnGameEnd += songPlayer.Pause;
    }

    private void OnDestroy()
    {
        DreamworldEventManager.OnGameStart -= songPlayer.Play;
        PauseManager.OnPaused -= TogglePause;
        DreamworldEventManager.OnGameEnd -= songPlayer.Pause;
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
