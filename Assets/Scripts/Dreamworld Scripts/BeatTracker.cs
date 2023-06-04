using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] private NoteTracker _noteTracker;
    private PlayerControl _playerControl;
    // Start is called before the first frame update
    void Start()
    {
        countdownStarted = false;
        enableCount = false;
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Dash.performed += context =>
        {
            //Lets the player start the level if they have not already done so
            if (!countdownStarted)
            {   
                onGameStart.Raise();
                welcomeMessage.SetActive(false);
                countdownStarted = true;
                enableCount = true;
            }
        };
        _playerControl.Enable();
        instance = this;
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
            _noteTracker.timeTracker += Time.deltaTime;  //Updates the script's understanding of how much time has passed since the player began the level
            //Checks if/when a new pair of "metronome bars" should appear
        }
    }
}


