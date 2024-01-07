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


    [SerializeField] private NoteTracker _noteTracker;
    // Start is called before the first frame update
    void Start()
    {
        countdownStarted = false;
        enableCount = true;
        instance = this;
        DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.ISPAUSED, Pause);
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.DIALOGUE_END, SetGameReady);
    }

    private void StartGame() 
    {
        if (!countdownStarted && enableCount)
        {   
            DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.GAME_START);
            welcomeMessage.SetActive(false);
            countdownStarted = true;
            DreamworldEventManager.Instance.DeregisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, StartGame);
        }
    }

    public void SetGameReady()
    {
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, StartGame);
    }

    public void Pause(bool value)
    {
        enableCount = !value;
    }

    // Update is called once per frame
    void Update()
    {
        //When the player has clicked/tapped to begin the level
        if(enableCount && countdownStarted)
        {
            if (_noteTracker.timeTracker >= _noteTracker.totalTime)
            {
                DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.GAME_END);
                DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_PAUSE);
                enableCount = false;
                return;
            }
            _noteTracker.timeTracker += Time.deltaTime;  //Updates the script's understanding of how much time has passed since the player began the level
        }
    }
}


