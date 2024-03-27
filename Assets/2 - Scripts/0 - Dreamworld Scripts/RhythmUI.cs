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
    [SerializeField] private bool _countdownFinished;
    private GameObject dreamworldUICanvas;

    public NoteTracker _NoteTracker;
    
    [SerializeField]
    private GameObject
        fadingMessageTextObject; //TextObject prefab that fades away briefly after appearing onscreen

    [SerializeField]
    private DashTutorialText
        dashTutorialTextObject; //TextObject prefab designed for tutorialization; disappears only after the player successfully completes a few dashes
    [SerializeField]
    private CollectableTutorialText
        collectableTutorialTextObject; //TextObject prefab designed for tutorialization; disappears only after the player successfully completes a few dashes

    private void Awake()
    {
        _NoteTracker.onLoad += Init;
        _NoteTracker.onTimeUpdate += HandleCountdown;
    }

    public void Init()
    {
        dreamworldUICanvas = DreamworldDialogueController.Instance.gameObject;
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
        if (_countdownFinished)
        {
            return;
        }
        float timeTracker = _NoteTracker.timeTracker;

        //"3..."
        if (timeTracker >= countdownTextTriggerTime3
            && timeTracker < countdownTextTriggerTime2)
        {
            if (_countdownTextDisplayed3 == false)
            {
                _countdownTextDisplayed3 = true;

                GameObject countdownText3 = Instantiate(fadingMessageTextObject,
                    dreamworldUICanvas.transform);
                (countdownText3.transform as RectTransform).localPosition = new Vector3(0, 0, 0);
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
                    dreamworldUICanvas.transform);
                (countdownText2.transform as RectTransform).localPosition = new Vector3(0, 0, 0);
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
                    dreamworldUICanvas.transform);
                (countdownText1.transform as RectTransform).localPosition = new Vector3(0, 0, 0);
                countdownText1.GetComponent<TMP_Text>().text = "1...";
            }

        

            //"Click / Tap to the Beat!"
        }
        else if (timeTracker >= countdownFinishTime)
        {
            if (_countdownFinished == false)
            {
                DashTutorialText countdownFinishedText = Instantiate(dashTutorialTextObject,
                    dreamworldUICanvas.transform);
                
                countdownFinishedText.Init(_NoteTracker, () =>
                {
                    CollectableTutorialText countdownFinishedText = Instantiate(collectableTutorialTextObject,
                        dreamworldUICanvas.transform);
                    (countdownFinishedText.gameObject.transform as RectTransform).localPosition =
                        new Vector3(0, 250, 0);
                });
                (countdownFinishedText.gameObject.transform as RectTransform).localPosition =
                    new Vector3(0, 250, 0);
                countdownFinishedText.gameObject.GetComponent<TMP_Text>().text = "Click / Tap to the Beat!";
                _NoteTracker.onTimeUpdate -= HandleCountdown;
                DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.COUNTDOWN_FINISH);
                _countdownFinished = true;
            }

        }

    }
}
