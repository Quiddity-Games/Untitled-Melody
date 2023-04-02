using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The script responsible for determining what the song's tempo is and using that information to control various aspects of the game.
/// </summary>
public class BeatTracker : MonoBehaviour
{
    public static BeatTracker instance; //Singleton

    public GameObject welcomeMessage;   //Text message that greets the player before they start the level
    private GameObject playerCanvas;    //Canvas parented to the player, used to display text UI that should be attached to the player
    private GameObject screenSpaceCanvas;

    public AnimationCurve linearCurve;  //Used for lerp calculations

    public AudioSource songPlayer;

    public float timeTracker;   //How much time has passed since the player started the level; incremented with each frame
    float startTime;    //The time at which the player started the level

    public float clockTime; //Used to track how much time left in the song the player has to complete the level
    public Image clockBar;

    public bool startedLevelCountdown;   //Changes based on whether/not the player has started the level

    float nextBeatTime; //The "location" in time of the next beat. Periodically pushed ahead as the music continues to reflect when the next beat will be.
    private float twoBeatsLength;   //The length in seconds between two beats in the song's tempo. (Important b/c the rhythm that matters for the dash mechanic occurs only once every *two* beats of the song's tempo)
    public float forgivenessRange;  //The amount of time in seconds that the player can be "early" or "late" in hitting the beat, but still have it count as a successful dash
    public bool onBeat; //States whether or not the player has accurately clicked/tapped to the most recent beat
    public float bpm;   //"Beats per minute" of the song
    bool playerDashedThisBeat;  //States whether/not the player successfully dashed on the previous beat
    public bool canClick;   //Determines if the player has "spent" their attempt to click/dash for the beat they're on

    //Variables associated with the rhythm indicator (the pair of bars that appears above the player's head)
    private float rhythmIndicatorTimer; //Used to track when the rhythm indicator should spawn a new set of "metronome bars"
    public static bool needNewRhythmIndicatorBars1;
    public static bool needNewRhythmIndicatorBars2;
    public GameObject bar;
    public bool barDebugMode;  //A debug tool to help the developer test the rhythm "forgiveness" value and see whether a pair of bars will count as a hit or not before clicking
    private Color barColor;
    GameObject newBarL;
    GameObject newBarR;

    //Times at which a series of text messages will appear onscreen as the player begins the level
    float messageATriggerTime;
    float messageBTriggerTime;
    float messageCTriggerTime;
    float countdownFinishTime;
    public GameObject fadingMessageTextObject;
    public GameObject dashTutorialTextObject;
    bool messageATriggered;
    bool messageBTriggered;
    bool messageCTriggered;
    bool countdownFinished;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        songPlayer = GetComponent<AudioSource>();
        songPlayer.Stop();
        songPlayer.volume = .5f;

        twoBeatsLength = 60f / bpm;
        nextBeatTime = twoBeatsLength + (twoBeatsLength / 4f);    //Added to have the "beat" land on the second and fourth beats of each measure
        rhythmIndicatorTimer -= ((8f * twoBeatsLength)); //Offsets rhythmIndicatorTimer so that the bars don't start appearing until the percussion beats of "wishing well" begin, roughly four measures in

        clockTime = songPlayer.clip.length;

        playerCanvas = GameObject.Find("PlayerCanvas");
        screenSpaceCanvas = GameObject.Find("Screen Space Canvas");

        canClick = true;

        messageATriggerTime = (0.5f * twoBeatsLength);
        messageBTriggerTime = (2.5f * twoBeatsLength);
        messageCTriggerTime = (4.5f * twoBeatsLength);
        countdownFinishTime = (8f * twoBeatsLength);
        messageATriggered = false;
        messageBTriggered = false;
        messageCTriggered = false;
        countdownFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        //When the player has clicked/tapped to begin the level
        if(startedLevelCountdown)
        {
            TriggerTextMessages();

            //Resets the level if the player presses the "R" key
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetLevel();
            }

            timeTracker += Time.deltaTime;  //Updates the script's understanding of how much time has passed

            //Sets the amount of the timer bar UI that's still filled in
            clockTime = songPlayer.clip.length - (Time.time - startTime);
            clockBar.fillAmount = clockTime / songPlayer.clip.length;

            //Checks if/when a new pair of "metronome bars" should appear
            UpdateRhythmIndicator();

            //Checks to see if the countdown is over
            if(countdownFinished)
            {
                //Checks if the player is clicking/tapping on the beat
                if(timeTracker > (nextBeatTime - forgivenessRange) && timeTracker < (nextBeatTime + forgivenessRange))
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Hit");

                            //Disabling combo functionality for now

                            //GameManager.instance.dashCombos++;  //Increments the player's combo number
                            //playerDashedThisBeat = true;    //Saved to look at next beat and determine if the player's combo value should be reset
                    }

                    barColor = Color.yellow;   //Sets what color the bar will be if the player clicks on this frame

                    onBeat = true;

                } else
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Miss");

                        //Disabling combo functionality for now
                        //GameManager.instance.dashCombos = 0;    //Resets the player's combo number
                    }

                    barColor = Color.red;

                    onBeat = false;
                }

                //Updates nextBeatLocation each time the current beat is passed
                if(timeTracker > nextBeatTime + forgivenessRange)
                {
                    nextBeatTime += twoBeatsLength;
                    canClick = true;    //Refreshes the player's attempt to click/dash
                }
            }

        } else
        {
            //Start music/level
            if (Input.GetMouseButtonDown(0))
            {
                welcomeMessage.SetActive(false);
                songPlayer.Play();
                startTime = Time.time;
                startedLevelCountdown = true;
            }
        }
        
    }

    /// <summary>
    /// Checks for, and triggers, any player-facing text messages when they're set to happen.
    /// </summary>
    void TriggerTextMessages()
    {
        if (timeTracker >= messageATriggerTime
            && timeTracker < messageBTriggerTime)
        {
            Debug.Log("Message A!");

            if(messageATriggered == false)
            {
                messageATriggered = true;

                GameObject messageA = Instantiate(fadingMessageTextObject, screenSpaceCanvas.GetComponent<Transform>());
                messageA.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                messageA.GetComponent<TMP_Text>().text = "3...";
            }

        } else if(timeTracker >= messageBTriggerTime
            && timeTracker < messageCTriggerTime)
        {
            Debug.Log("Message B!");

            if(messageBTriggered == false)
            {
                messageBTriggered = true;

                GameObject messageB = Instantiate(fadingMessageTextObject, screenSpaceCanvas.GetComponent<Transform>());
                messageB.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                messageB.GetComponent<TMP_Text>().text = "2...";
            }

        } else if(timeTracker >= messageCTriggerTime
          && timeTracker < countdownFinishTime)
        {
            Debug.Log("Message C!");

            if(messageCTriggered == false)
            {
                messageCTriggered = true;

                GameObject messageC = Instantiate(fadingMessageTextObject, screenSpaceCanvas.GetComponent<Transform>());
                messageC.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                messageC.GetComponent<TMP_Text>().text = "1...";
            }

        } else if(timeTracker >= countdownFinishTime)
        {
            Debug.Log("Message D!");

            if(countdownFinished == false)
            {
                countdownFinished = true;

                GameObject messageD = Instantiate(dashTutorialTextObject, screenSpaceCanvas.GetComponent<Transform>());
                messageD.GetComponent<Transform>().localPosition = new Vector3(0, 120, 0);
                messageD.GetComponent<TMP_Text>().text = "Click / Tap to the Beat!";
            }
        }
    }

    /// <summary>
    /// Checks if/when a new pair of "metronome bars" should appear.
    /// </summary>
    void UpdateRhythmIndicator()
    {
        needNewRhythmIndicatorBars1 = false;
        needNewRhythmIndicatorBars2 = false;
        rhythmIndicatorTimer += Time.deltaTime;

        if(rhythmIndicatorTimer >= twoBeatsLength / 2
            && newBarL == null
            && newBarR == null)
        {
            needNewRhythmIndicatorBars1 = true;
        }

        if(rhythmIndicatorTimer >= twoBeatsLength)
        {
            rhythmIndicatorTimer -= twoBeatsLength;
            needNewRhythmIndicatorBars2 = true;

            //Disabling combo functionality for now
            /*
            if(playerDashedThisBeat == false)
            {
                GameManager.instance.dashCombos = 0;
            } else
            {
                playerDashedThisBeat = false;
            }
            */
        }

        if(needNewRhythmIndicatorBars1)
        {
            newBarL = Instantiate(bar, new Vector3(-4f, 4, 0), Quaternion.identity);
            newBarL.GetComponent<RectTransform>().SetParent(playerCanvas.transform, false);
            newBarL.GetComponent<RectTransform>().anchoredPosition = new Vector3(-4f, 4, 0);

            newBarR = Instantiate(bar, new Vector3(4f, 4, 0), Quaternion.identity);
            newBarR.GetComponent<RectTransform>().SetParent(playerCanvas.transform, false);
            newBarR.GetComponent<RectTransform>().anchoredPosition = new Vector3(4f, 4, 0);

        }

        //Spawns "metronome bar" UI to help the player visualize when the beat is
        if(needNewRhythmIndicatorBars2)
        {
            //StartCoroutine(RhythmIndicatorBarVisual(new Vector3(-4f, 4, 0)));
            //StartCoroutine(RhythmIndicatorBarVisual(new Vector3(4f, 4, 0)));

            StartCoroutine(MoveRhythmIndicatorBarVisual(newBarL));
            StartCoroutine(MoveRhythmIndicatorBarVisual(newBarR));
        }
    }

    /*
    /// <summary>
    /// Spawns a new pair of "metronome bars" to help the player visualize the rhythm.
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    IEnumerator RhythmIndicatorBarVisual(Vector3 startPos)
    {
        GameObject newBar = Instantiate(bar, startPos, Quaternion.identity);
        newBar.GetComponent<RectTransform>().SetParent(playerCanvas.transform, false);
        newBar.GetComponent<RectTransform>().anchoredPosition = startPos;

        Vector3 endPos = new Vector3(0, startPos.y,0);

        bool instaDestroyBar = true;   //Used to determine whether/not a bar should be instantly destroyed, or allowed to fade away in place for a moment (based on if player has clicked or not)

        float t = 0;

        while (t < 1)
        {
            newBar.GetComponent<RectTransform>().anchoredPosition = Vector3.LerpUnclamped(startPos, endPos, linearCurve.Evaluate(t));
            t += Time.deltaTime * twoBeatsLength;
            //t += Time.deltaTime / beatInterval;

            //Changes bar color _before_ player clicks, but only if debug mode is on
            if(barDebugMode == true)
            {
                newBar.GetComponent<Image>().color = barColor;
            }

            if(Input.GetMouseButtonDown(0))
            {
                newBar.GetComponent<Image>().color = barColor;    //Changes the bar's color after the player has clicked, so they can see whether/not they hit it
                t = 1;

                instaDestroyBar = false;
            }

            yield return 0;
        }

        //Either destroys the bars, or causes it to fade away _then_ destroy, depending on whether or not the player "hits" it before it disappears
        if(instaDestroyBar == true)
        {
            Destroy(newBar);

        } else
        {
            float alpha = newBar.GetComponent<Image>().color.a;

            while(alpha >= 0)
            {
                newBar.GetComponent<Image>().color = new Color(newBar.GetComponent<Image>().color.r, newBar.GetComponent<Image>().color.g, newBar.GetComponent<Image>().color.b, alpha);
                alpha -= 0.02f; //Note to Self: Maybe for faster rhythms/songs, the alpha should fade faster, b/c more bars are coming in faster?

                yield return 0;
            }

            Destroy(newBar);
        }

        yield return 0;
    }
    */

    /// <summary>
    /// New coroutine, WIP!
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    IEnumerator MoveRhythmIndicatorBarVisual(GameObject bar)
    {
        Vector3 startPos = bar.GetComponent<Transform>().localPosition;
        Vector3 endPos = new Vector3(0, startPos.y, 0);

        bool instaDestroyBar = true;   //Used to determine whether/not a bar should be instantly destroyed, or allowed to fade away in place for a moment (based on if player has clicked or not)

        float t = 0;

        while(t < 1)
        {
            bar.GetComponent<RectTransform>().anchoredPosition = Vector3.LerpUnclamped(startPos, endPos, linearCurve.Evaluate(t));

            t += Time.deltaTime / (twoBeatsLength/4);
            //t += Time.deltaTime / beatInterval;

            //Changes bar color _before_ player clicks, but only if debug mode is on
            if(barDebugMode == true)
            {
                bar.GetComponent<Image>().color = barColor;
            }

            if(Input.GetMouseButtonDown(0))
            {

                bar.GetComponent<Image>().color = barColor;    //Changes the bar's color after the player has clicked, so they can see whether/not they hit it

                t = 1;

                instaDestroyBar = false;
            }

            yield return 0;
        }

        //Either destroys the bars, or causes it to fade away _then_ destroy, depending on whether or not the player "hits" it before it disappears
        if(instaDestroyBar == true)
        {
            Destroy(bar);

        }
        else
        {
            float alpha = bar.GetComponent<Image>().color.a;

            while(alpha >= 0)
            {
                bar.GetComponent<Image>().color = new Color(bar.GetComponent<Image>().color.r, bar.GetComponent<Image>().color.g, bar.GetComponent<Image>().color.b, alpha);
                alpha -= 0.02f; //Note to Self: Maybe for faster rhythms/songs, the alpha should fade faster, b/c more bars are coming in faster?

                yield return 0;
            }

            Destroy(bar);
        }

        yield return 0;
    }

    /// <summary>
    /// Resets the level.
    /// </summary>
    public void ResetLevel()
    {
        Debug.Log("Restart triggered!");
        clockTime = songPlayer.clip.length;
    }
}
