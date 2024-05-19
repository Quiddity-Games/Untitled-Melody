using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{

    [SerializeField] private AudioSource songPlayer;

    private void OnEnable()
    {
        DreamworldEventManager.OnGameStart += songPlayer.Play;
        PauseMenuManager.OnPaused += TogglePause;
        DreamworldEventManager.OnGameEnd += () => TogglePause(true);
    }

    private void OnDestroy()
    {
        DreamworldEventManager.OnGameStart -= songPlayer.Play;
        PauseMenuManager.OnPaused -= TogglePause;
        DreamworldEventManager.OnGameEnd -= () => TogglePause(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //DreamworldEventManager.Instance.RegisterVoidEventResponse(
        //    DreamworldVoidEventEnum.GAME_START, Play);
        //DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.ISPAUSED,
        //    TogglePause);
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
