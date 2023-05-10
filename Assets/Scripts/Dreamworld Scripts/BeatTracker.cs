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

    public bool onBeat; //States whether or not the player has accurately clicked/tapped to the most recent beat


    bool playerDashedThisBeat;  //States whether/not the player successfully dashed on the previous beat
    public bool CanDash;   //Determines if the player has "spent" their attempt to click/dash for the beat (of the matchable rhythm) that they're currently on

    private bool startedLevelCountdown;

    public GameEvent onGameStart;

    [SerializeField] private MetronomeBarController _metronomeBar;
    [SerializeField] private NoteTracker _noteTracker;
    private PlayerControl _playerControl;
    // Start is called before the first frame update
    void Start()
    {
        startedLevelCountdown = false;
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Dash.performed += context =>
        {
            //Lets the player start the level if they have not already done so
            if (!startedLevelCountdown)
            {   
                onGameStart.Raise();
                welcomeMessage.SetActive(false);
                startedLevelCountdown = true;
            }
        };
        _playerControl.Enable();

        //TODO: Move to a Dash Handler
        _noteTracker.offBeatTrigger += () => CanDash = true;
        instance = this;
        CanDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        //When the player has clicked/tapped to begin the level
        if(startedLevelCountdown)
        {
            _noteTracker.timeTracker += Time.deltaTime;  //Updates the script's understanding of how much time has passed since the player began the level
            //Checks if/when a new pair of "metronome bars" should appear
            _metronomeBar.UpdateRhythmIndicator();
        }
    }
}


