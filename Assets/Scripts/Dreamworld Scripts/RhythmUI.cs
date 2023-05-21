using RoboRyanTron.Unite2017.Events;
using TMPro;
using UnityEngine;

public class RhythmUI : MonoBehaviour
{

    //Times at which a series of "countdown to start" text objects will appear onscreen as the player begins the level
    [SerializeField] private float countdownTextTriggerTime3;
    [SerializeField]private float countdownTextTriggerTime2;
    [SerializeField]private float countdownTextTriggerTime1;
    [SerializeField]private float countdownFinishTime;

    //Bools noting if a particular part of the "countdown to start" has already been shown or not
    bool _countdownTextDisplayed3;
    bool _countdownTextDisplayed2;
    private bool _countdownTextDisplayed1;
    private bool _countdownFinished;
    private GameObject screenSpaceCanvas;

    public GameEvent countdownOver;
    public NoteTracker _NoteTracker;
    
    [SerializeField]
    private GameObject
        fadingMessageTextObject; //TextObject prefab that fades away briefly after appearing onscreen

    [SerializeField]
    private GameObject
        dashTutorialTextObject; //TextObject prefab designed for tutorialization; disappears only after the player successfully completes a few dashes

    private void Start()
    {
        screenSpaceCanvas = GameObject.Find("Screen Space Canvas");
      
        
        _NoteTracker.onTimeUpdate += HandleCountdown;
    }

    public void Init()
    {
        countdownTextTriggerTime3 = (0.5f * _NoteTracker.GetTwoBeatsLength());
        countdownTextTriggerTime2 = (2.5f * _NoteTracker.GetTwoBeatsLength());
        countdownTextTriggerTime1 = (4.5f * _NoteTracker.GetTwoBeatsLength());
        countdownFinishTime = (8f * _NoteTracker.GetTwoBeatsLength());
        _countdownTextDisplayed3 = false;
        _countdownTextDisplayed2 = false;
        _countdownTextDisplayed1 = false;
        _countdownFinished = false;
    }



    /// <summary>
    /// Checks for, and triggers, any player-facing text UI messages when they're set to happen.
    /// </summary>
    public void HandleCountdown()
    {
        float timeTracker = _NoteTracker.timeTracker;

        //"3..."
        if (timeTracker >= countdownTextTriggerTime3
            && timeTracker < countdownTextTriggerTime2)
        {
            if (_countdownTextDisplayed3 == false)
            {
                _countdownTextDisplayed3 = true;

                GameObject countdownText3 = Instantiate(fadingMessageTextObject,
                    screenSpaceCanvas.GetComponent<Transform>());
                countdownText3.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                countdownText3.GetComponent<TMP_Text>().text = "3...";
            }

            //"2..."
        }
        else if (timeTracker >= countdownTextTriggerTime2
                 && timeTracker < countdownTextTriggerTime1)
        {
            if (_countdownTextDisplayed2 == false)
            {
                _countdownTextDisplayed2 = true;

                GameObject countdownText2 = Instantiate(fadingMessageTextObject,
                    screenSpaceCanvas.GetComponent<Transform>());
                countdownText2.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                countdownText2.GetComponent<TMP_Text>().text = "2...";
            }


            //"1..."
        }
        else if (timeTracker >= countdownTextTriggerTime1
                 && timeTracker < countdownFinishTime)
        {
            if (_countdownTextDisplayed1 == false)
            {
                _countdownTextDisplayed1 = true;

                GameObject countdownText1 = Instantiate(fadingMessageTextObject,
                    screenSpaceCanvas.GetComponent<Transform>());
                countdownText1.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                countdownText1.GetComponent<TMP_Text>().text = "1...";
            }

        

            //"Click / Tap to the Beat!"
        }
        else if (timeTracker >= countdownFinishTime)
        {
            if (_countdownFinished == false)
            {
                GameObject countdownFinishedText = Instantiate(dashTutorialTextObject,
                    screenSpaceCanvas.GetComponent<Transform>());
                countdownFinishedText.GetComponent<Transform>().localPosition =
                    new Vector3(0, 120, 0);
                countdownFinishedText.GetComponent<TMP_Text>().text = "Click / Tap to the Beat!";
                _NoteTracker.onTimeUpdate -= HandleCountdown;
                countdownOver.Raise();
            }

        }

    }
}
