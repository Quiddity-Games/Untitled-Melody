using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// The script responsible for determining what the song's tempo is and using that information to control various aspects of the game. Attached to the BeatTracker gameObject.
/// </summary>
public class BeatTracker : MonoBehaviour
{
    public static BeatTracker instance; //Singleton

    public GameObject welcomeMessage;   //Text message that greets the player before they start the level



    bool playerDashedThisBeat;  //States whether/not the player successfully dashed on the previous beat

    private bool enableCount;

    private bool countdownStarted;

    public GameEvent onGameStart;
    [SerializeField] private GameEvent OnGameEnd;
    [SerializeField] private GameEvent onDialogueEnd;

    [SerializeField] private NoteTracker _noteTracker;
    private PlayerControl _playerControl;
    // Start is called before the first frame update
    void Start()
    {
        countdownStarted = false;
        enableCount = true;
        _playerControl = new PlayerControl();
        //_playerControl.Dreamworld.Dash.performed +=  StartGame;
            _playerControl.Enable();
        instance = this;
    }

 

    private void StartGame(InputAction.CallbackContext ctx) 
    {
        if (!countdownStarted && enableCount)
        {   
            onGameStart.Raise();
            welcomeMessage.SetActive(false);
            countdownStarted = true;
            _playerControl.Dreamworld.Dash.performed -= StartGame;
        }
    }

    public void SetGameReady()
    {
        _playerControl.Dreamworld.Dash.performed += StartGame;
    }

    public void Pause(bool value)
    {
        enableCount = value;
    }

    // Update is called once per frame
    void Update()
    {
        //When the player has clicked/tapped to begin the level
        if(enableCount && countdownStarted)
        {
            if (_noteTracker.timeTracker >= _noteTracker.totalTime)
            {
                OnGameEnd.Raise();
                enableCount = false;
                return;
            }
            _noteTracker.timeTracker += Time.deltaTime;  //Updates the script's understanding of how much time has passed since the player began the level
        }
    }
}


