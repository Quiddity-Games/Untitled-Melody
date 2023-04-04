using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script responsible for pulling up the pause menu. Attached to the GameManager gameObject.
/// </summary>
public class Menus : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool paused;

    // Update is called once per frame
    void Update()
    {
        //Pauses the game if the Esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            BeatTracker.instance.songPlayer.Pause();
            pauseMenu.SetActive(true);

        //Unpausing
        } else if (Input.GetKeyDown(KeyCode.Escape) && paused) 
        {
            paused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;

            if(BeatTracker.instance.startedLevelCountdown == true)
            {
                BeatTracker.instance.songPlayer.Play();
            }
        }
    }

    /// <summary>
    /// Called by a button to increase the volume of the game's music.
    /// </summary>
    public void IncreaseVolume()
    {
        BeatTracker.instance.songPlayer.volume += .1f;
    }

    /// <summary>
    /// Called by a button to lower the volume of the game's music.
    /// </summary>
    public void DecreaseVolume()
    {
        BeatTracker.instance.songPlayer.volume -= .1f;
    }
}
