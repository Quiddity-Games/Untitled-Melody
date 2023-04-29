using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private PlayerControl _playerControl;

    // Start is called before the first frame update
    void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Pause.performed += context =>
        {
            if (!Menus.paused)
            {
                Menus.paused = true;
                Time.timeScale = 0;
                BeatTracker.instance.songPlayer.Pause();
                //Menus.pauseMenu.SetActive(true);

                //Unpausing
            }
            else
            {
                Menus.paused = false;
                //Menus.pauseMenu.SetActive(false);
                Time.timeScale = 1;

                if (BeatTracker.instance.startedLevelCountdown == true)
                {
                    BeatTracker.instance.songPlayer.Play();
                }
            }
        };
    }
}
