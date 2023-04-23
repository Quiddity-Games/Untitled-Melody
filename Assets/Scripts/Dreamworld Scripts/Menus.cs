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

    public void OnGamePause(bool onPause)
    {
        paused = onPause;
        pauseMenu.SetActive(onPause);
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