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

    public float clockTime; //Used to track how much time is left in the song, and thereby how much time the player has to complete the level
    public Image clockBar;  //The UI asset displaying the player's remaining time

    public bool startedLevelCountdown;   //Determines whether/not the player has started the level

    float nextBeatTime; //The "location" in time of the next beat that the player can hit/miss, aka the "matchable rhythm" for the dash mechanic
    private float twoBeatsLength;   //The length in seconds between two beats in the song's tempo. (Important b/c the "matchable rhythm" for the dash mechanic occurs only once every *two* beats of the song's tempo)
    public float forgivenessRange;  //The amount of time in seconds that the player can be "early" or "late" in hitting the beat, but still have it count as a successful hit

    public bool onBeat; //States whether or not the player has accurately clicked/tapped to the most recent beat
    public float bpm;   //"Beats per minute" of the song

    bool playerDashedThisBeat;  //States whether/not the player successfully dashed on the previous beat
    public bool canDash;   //Determines if the player has "spent" their attempt to click/dash for the beat (of the matchable rhythm) that they're currently on

    private float rhythmIndicatorTimer; //Timer specifically dedicated to the rhythm indicator, aka the "metronome bars" above the player's head
    public static bool spawnNewMetronomeBars;   //Determines when the rhythm indicator should spawn a new set of (initially unmoving) metronome bars
    public static bool startMovingMetronomeBars;    //Determines when those metronome bars should start moving towards e/o

    public GameObject metronomeBar;  //Standard prefab for a metronome bar
    private Color metronomeBarColor;    //Metronome bars change color as the player clicks/taps to show whether they player "hit" or "missed" the beat
    public bool metronomeBarDebugMode;  //Lets the dev test the rhythm "forgiveness" value by having the bars change color to show if they _will_ be hits/missed _before_ the player even clicks/taps
    GameObject newMetronomeBarL;
    GameObject newMetronomeBarR;

    //Times at which a series of "countdown to start" text objects will appear onscreen as the player begins the level
    float countdownTextTriggerTime3;
    float countdownTextTriggerTime2;
    float countdownTextTriggerTime1;
    float countdownFinishTime;

    //Bools noting if a particular part of the "countdown to start" has already been shown or not
    bool countdownTextDisplayed3;
    bool countdownTextDisplayed2;
    bool countdownTextDisplayed1;
    bool countdownFinished;

    public GameObject fadingMessageTextObject;  //TextObject prefab that fades away briefly after appearing onscreen
    public GameObject dashTutorialTextObject;   //TextObject prefab designed for tutorialization; disappears only after the player successfully completes a few dashes

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        songPlayer = GetComponent<AudioSource>();
        songPlayer.Stop();
        songPlayer.volume = .5f;

        twoBeatsLength = 60f / bpm;
        nextBeatTime = twoBeatsLength + (twoBeatsLength / 4f);    //Added to have the "matchable rhythm" land on the second and fourth beats of each measure
        rhythmIndicatorTimer -= ((8f * twoBeatsLength)); //Offsets rhythmIndicatorTimer so that the "metronome bars" above the player's head don't start appearing until the percussion beats of the "wishing well" song begin, roughly four measures in

        clockTime = songPlayer.clip.length;

        playerCanvas = GameObject.Find("PlayerCanvas");
        screenSpaceCanvas = GameObject.Find("Screen Space Canvas");

        canDash = true;

        countdownTextTriggerTime3 = (0.5f * twoBeatsLength);
        countdownTextTriggerTime2 = (2.5f * twoBeatsLength);
        countdownTextTriggerTime1 = (4.5f * twoBeatsLength);
        countdownFinishTime = (8f * twoBeatsLength);
        countdownTextDisplayed3 = false;
        countdownTextDisplayed2 = false;
        countdownTextDisplayed1 = false;
        countdownFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        //When the player has clicked/tapped to begin the level
        if(startedLevelCountdown)
        {
            TriggerUIText();

            //Resets the level if the player presses the "R" key
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetLevel();
            }

            timeTracker += Time.deltaTime;  //Updates the script's understanding of how much time has passed since the player began the level

            //Sets the amount of the timer bar UI that should still be filled in
            clockTime = songPlayer.clip.length - (Time.time - startTime);
            clockBar.fillAmount = clockTime / songPlayer.clip.length;

            //Checks if/when a new pair of "metronome bars" should appear
            UpdateRhythmIndicator();

            //Checks to see if the countdown-to-start is over
            if(countdownFinished)
            {
                //Checks to see if the player clicking/tapping during this frame would count as being "on-beat"/successful
                if(timeTracker > (nextBeatTime - forgivenessRange) && timeTracker < (nextBeatTime + forgivenessRange))
                {
                    //Checks to see if the player has _actually_ clicked/tapped on this frame
                    if(Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Hit the beat");

                        //Used to increment a combo tracker, currently disabled due to playtesting feedback
                        //GameManager.instance.dashCombos++;  //Increments the player's combo number
                        //playerDashedThisBeat = true;    //Saved to look at next beat and determine if the player's combo value should be reset
                    }

                    metronomeBarColor = Color.yellow;   //Sets what color the bar will be if the player clicks/taps on this frame

                    onBeat = true;

                } else
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Missed the beat");

                        //Combo functionality, currently disabled due to playtesting feedback
                        //GameManager.instance.dashCombos = 0;    //Resets the player's combo number
                    }

                    metronomeBarColor = Color.red;

                    onBeat = false;
                }

                //Determines when the next beat of the matchable rhythm will be
                if(timeTracker > nextBeatTime + forgivenessRange)
                {
                    nextBeatTime += twoBeatsLength;
                    canDash = true;    //Refreshes the player's attempt to dash
                }
            }

        } else
        {
            //Lets the player start the level if they have not already done so
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
    /// Checks for, and triggers, any player-facing text UI messages when they're set to happen.
    /// </summary>
    void TriggerUIText()
    {
        //"3..."
        if (timeTracker >= countdownTextTriggerTime3
            && timeTracker < countdownTextTriggerTime2)
        {
            if(countdownTextDisplayed3 == false)
            {
                countdownTextDisplayed3 = true;

                GameObject countdownText3 = Instantiate(fadingMessageTextObject, screenSpaceCanvas.GetComponent<Transform>());
                countdownText3.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                countdownText3.GetComponent<TMP_Text>().text = "3...";
            }

        //"2..."
        } else if(timeTracker >= countdownTextTriggerTime2
            && timeTracker < countdownTextTriggerTime1)
        {
            if(countdownTextDisplayed2 == false)
            {
                countdownTextDisplayed2 = true;

                GameObject countdownText2 = Instantiate(fadingMessageTextObject, screenSpaceCanvas.GetComponent<Transform>());
                countdownText2.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                countdownText2.GetComponent<TMP_Text>().text = "2...";
            }

        //"1..."
        } else if(timeTracker >= countdownTextTriggerTime1
          && timeTracker < countdownFinishTime)
        {
            if(countdownTextDisplayed1 == false)
            {
                countdownTextDisplayed1 = true;

                GameObject countdownText1 = Instantiate(fadingMessageTextObject, screenSpaceCanvas.GetComponent<Transform>());
                countdownText1.GetComponent<Transform>().localPosition = new Vector3(0, 64, 0);
                countdownText1.GetComponent<TMP_Text>().text = "1...";
            }

        //"Click / Tap to the Beat!"
        } else if(timeTracker >= countdownFinishTime)
        {
            if(countdownFinished == false)
            {
                countdownFinished = true;

                GameObject countdownFinishedText = Instantiate(dashTutorialTextObject, screenSpaceCanvas.GetComponent<Transform>());
                countdownFinishedText.GetComponent<Transform>().localPosition = new Vector3(0, 120, 0);
                countdownFinishedText.GetComponent<TMP_Text>().text = "Click / Tap to the Beat!";
            }
        }
    }

    /// <summary>
    /// Checks if/when a new pair of "metronome bars" should appear.
    /// </summary>
    void UpdateRhythmIndicator()
    {
        spawnNewMetronomeBars = false;
        startMovingMetronomeBars = false;

        rhythmIndicatorTimer += Time.deltaTime;

        //Triggers when it's time for a new pair of metronome bars to appear
        if(rhythmIndicatorTimer >= twoBeatsLength / 2
            && newMetronomeBarL == null
            && newMetronomeBarR == null)
        {
            spawnNewMetronomeBars = true;
        }

        //Triggers when it's time for those metronome bars to start moving towards each other
        if(rhythmIndicatorTimer >= twoBeatsLength)
        {
            rhythmIndicatorTimer -= twoBeatsLength;
            startMovingMetronomeBars = true;

            //Code previously used for tracking combos, disabled for now (though not sure why this is down here in this part of the script, TBH)
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

        //Creates a new pair of metronome bars
        if(spawnNewMetronomeBars)
        {
            newMetronomeBarL = Instantiate(metronomeBar, new Vector3(-4f, 4, 0), Quaternion.identity);
            newMetronomeBarL.GetComponent<RectTransform>().SetParent(playerCanvas.transform, false);
            newMetronomeBarL.GetComponent<RectTransform>().anchoredPosition = new Vector3(-4f, 4, 0);

            newMetronomeBarR = Instantiate(metronomeBar, new Vector3(4f, 4, 0), Quaternion.identity);
            newMetronomeBarR.GetComponent<RectTransform>().SetParent(playerCanvas.transform, false);
            newMetronomeBarR.GetComponent<RectTransform>().anchoredPosition = new Vector3(4f, 4, 0);

        }

        //Tells the current pair of metronome bars to start moving
        if(startMovingMetronomeBars)
        {
            StartCoroutine(MoveRhythmIndicatorBarVisual(newMetronomeBarL));
            StartCoroutine(MoveRhythmIndicatorBarVisual(newMetronomeBarR));
        }
    }

    //The old function/coroutine for spawning/moving metronome bars from a version of the mechanic where they started moving as soon as they were spawned. Left here in case useful to refer back to later.
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
    /// The function/coroutine that tells the two metronome bars currently onscreen to start moving towards each other.
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    IEnumerator MoveRhythmIndicatorBarVisual(GameObject bar)
    {
        Vector3 startPos = bar.GetComponent<Transform>().localPosition;
        Vector3 endPos = new Vector3(0, startPos.y, 0);

        bool instaDestroyBar = true;   //Used to determine whether/not a bar should be instantly destroyed (if the bar completed its movement without the player clicking/tapping in time), or freeze and fade away in place (if the player clicked/tapped before the bar disappeared)

        float t = 0;

        while(t < 1)
        {
            bar.GetComponent<RectTransform>().anchoredPosition = Vector3.LerpUnclamped(startPos, endPos, linearCurve.Evaluate(t));

            t += Time.deltaTime / (twoBeatsLength/4);

            //Changes bar color _before_ player clicks -- to give away if it will be a hit/not -- but only if debug mode is on
            if(metronomeBarDebugMode == true)
            {
                bar.GetComponent<Image>().color = metronomeBarColor;
            }

            if(Input.GetMouseButtonDown(0))
            {

                bar.GetComponent<Image>().color = metronomeBarColor;    //Changes the bar's color after the player has clicked, so they can see whether/not they hit it

                t = 1;

                instaDestroyBar = false;
            }

            yield return 0;
        }

        //Either destroys the bar, or causes it to fade away _then_ destroy, depending on whether or not the player "hits" it before it disappears
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
                alpha -= 0.02f; //Future Revision Note: Maybe for faster rhythms/songs, the alpha should fade faster, b/c more bars are coming in faster?

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
