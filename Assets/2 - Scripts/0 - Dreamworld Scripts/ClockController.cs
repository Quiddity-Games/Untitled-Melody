using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    
    public float clockTime; //Used to track how much time is left in the song, and thereby how much time the player has to complete the level
    public Image clockBar;  //The UI asset displaying the player's remaining time

    [SerializeField] private NoteTracker _noteTracker;

    private void Start()
    {
        _noteTracker.onTimeUpdate += UpdateDisplay;
    }

    // Update is called once per frame
    void UpdateDisplay()
    {
        //Sets the amount of the timer bar UI that should still be filled in

        clockBar.fillAmount = _noteTracker.timeTracker / _noteTracker.totalTime;
    }
}
