using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private ClickManager _clickManager;

    void Update()
    {
        _clickManager.CursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

        // Determine if the player wants to dash, and hasn't spent their dash yet
        if (Input.GetKeyDown(KeyCode.Mouse0) && BeatTracker.instance.CanDash)
        {
            BeatTracker.instance.CanDash = false;   //Player's click is "spent" until the next beat
            _clickManager.HandleClick();
        }

        if (Input.GetKeyDown(KeyCode.R) && !Menus.paused)
        {
            GameManager.instance.numCollectables = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //Pauses the game if the Esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && !Menus.paused)
        {
            Menus.paused = true;
            Time.timeScale = 0;
            BeatTracker.instance.songPlayer.Pause();
            //Menus.pauseMenu.SetActive(true);

            //Unpausing
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Menus.paused)
        {
            Menus.paused = false;
            //Menus.pauseMenu.SetActive(false);
            Time.timeScale = 1;

            if (BeatTracker.instance.startedLevelCountdown == true)
            {
                BeatTracker.instance.songPlayer.Play();
            }
        }
    }
}
