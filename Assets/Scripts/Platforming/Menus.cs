using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool paused;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            BeatTracker.instance.songPlayer.Pause();
            pauseMenu.SetActive(true);
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

    public void IncreaseVolume()
    {
        BeatTracker.instance.songPlayer.volume += .1f;
    }
    public void DecreaseVolume()
    {
        BeatTracker.instance.songPlayer.volume -= .1f;
    }
}
