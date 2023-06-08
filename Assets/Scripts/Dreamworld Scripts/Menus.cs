using System;
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
    public BoolVariable pause;

    public FloatVariable volume;
    private void Awake()
    {
        pause.OnValueChange += OnGamePause;
    }

    private void OnDestroy()
    {
        pause.OnValueChange -= OnGamePause;
    }

    public void OnGamePause()
    {
        pauseMenu.SetActive(pause.Value);
    }

    /// <summary>
    /// Called by a button to increase the volume of the game's music.
    /// </summary>
    public void IncreaseVolume()
    {
        volume.Value += .1f;
        //BeatTracker.instance.songPlayer.volume += .1f;
    }

    /// <summary>
    /// Called by a button to lower the volume of the game's music.
    /// </summary>
    public void DecreaseVolume()
    {
        volume.Value -= .1f;
        //  BeatTracker.instance.songPlayer.volume -= .1f;
    }
}