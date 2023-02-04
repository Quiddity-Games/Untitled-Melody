using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatTracker : MonoBehaviour
{
    public static BeatTracker instance;

    public GameObject welcomeMessage;
    public AnimationCurve linearCurve;
    private GameObject playerCanvas;
    public AudioSource songPlayer;
    public bool startedLevel;

    public float timeTracker;
    float nextBeatLocation; //The "location" of the next beat on the timeline. Periodically pushed ahead as the music continues in order to reflect when the next beat will be.
    private float beatLength;   //The length in seconds between each beat
    public float forgivenessRange;  //The amount of time in seconds that the player can be "early" or "late" in hitting the beat, but still have it count as a successful dash
    public bool onBeat;
    public float bpm;
    bool playerDashedThisBeat;  //States whether/not the player successfully dashed on the previous beat

    public TMP_Text clock;
    public float clockTime;
    public Image clockBar;
    public bool canClick;   //Determines if the player has "spent" their attempt to click/dash for the beat they're on
    float startTime;

    private float rhythmIndicatorTimer;
    public static bool needNewRhythmIndicatorBars;
    public GameObject bar;
    public bool barDebugMode;  //A debug tool to help the developer test the rhythm "forgiveness" value and see whether a pair of bars will count as a hit or not before clicking
    private Color barDebugColor;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        songPlayer = GetComponent<AudioSource>();
        songPlayer.Stop();
        songPlayer.volume = .5f;

        beatLength = 60f / bpm;
        nextBeatLocation = beatLength + (beatLength / 4f);    //Added to have the "beat" land on the second and fourth beats of each measure
        rhythmIndicatorTimer -= (beatLength / 2f) + 1f; //Offsets rhythmIndicatorTimer so that the bars of the rhythm indicator collide right on the beat

        clockTime = songPlayer.clip.length;
        playerCanvas = GameObject.Find("PlayerCanvas");
        canClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedLevel)
        {
            //Resets the level if the player presses the "R" key. (This can also be done by clicking/tapping a button on the game screen)
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetLevel();
            }

            clockTime = songPlayer.clip.length - (Time.time - startTime);
            //Debug.Log(clockTime);
            clock.text = "" + clockTime;
            clockBar.fillAmount = clockTime / songPlayer.clip.length;
            timeTracker += Time.deltaTime;
            
            //Checks if/when a new pair of "metronome bars" should appear
            UpdateRhythmIndicator();

            //Checks if the player is clicking/tapping on the beat
            if (timeTracker > (nextBeatLocation - forgivenessRange) && timeTracker < (nextBeatLocation + forgivenessRange))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Hit");

                    //Disabling combo functionality for now
                    //GameManager.instance.dashCombos++;  //Increments the player's combo number
                    //playerDashedThisBeat = true;    //Saved to look at next beat and determine if the player's combo value should be reset
                }

                //Sets what color the bar will be if the player clicks on this frame
                barDebugColor = Color.yellow;

                onBeat = true;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Miss");

                    //Disabling combo functionality for now
                    //GameManager.instance.dashCombos = 0;    //Resets the player's combo number
                }

                barDebugColor = Color.red;

                onBeat = false;
            }

            //Updates nextBeatLocation each time the current beat is passed
            if (timeTracker > nextBeatLocation + forgivenessRange)
            {
                nextBeatLocation += beatLength;
                canClick = true;    //Refreshes the player's attempt to click/dash
            }

        } else
        {
            //Start Song
            if (Input.GetMouseButtonDown(0))
            {
                welcomeMessage.SetActive(false);
                songPlayer.Play();
                startTime = Time.time;
                startedLevel = true;
            }
        }
        
    }

    /// <summary>
    /// Checks if/when a new pair of "metronome bars" should appear.
    /// </summary>
    void UpdateRhythmIndicator()
    {
        needNewRhythmIndicatorBars = false;
        rhythmIndicatorTimer += Time.deltaTime;

        if(rhythmIndicatorTimer >= beatLength)
        {
            rhythmIndicatorTimer -= beatLength;
            needNewRhythmIndicatorBars = true;

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

        //Spawns "metronome bar" UI to help the player visualize when the beat is
        if(needNewRhythmIndicatorBars)
        {
            StartCoroutine(RhythmIndicatorBarVisual(new Vector3(-4f, 4, 0)));
            StartCoroutine(RhythmIndicatorBarVisual(new Vector3(4f, 4, 0)));
        }
    }

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
            t += Time.deltaTime * beatLength;
            //t += Time.deltaTime / beatInterval;

            //Changes bar color _before_ player clicks, but only if debug mode is on
            if(barDebugMode == true)
            {
                newBar.GetComponent<Image>().color = barDebugColor;
            }

            if(Input.GetMouseButtonDown(0))
            {
                newBar.GetComponent<Image>().color = barDebugColor;    //Changes the bar's color after the player has clicked, so they can see whether/not they hit it
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

    /// <summary>
    /// Resets the level.
    /// </summary>
    public void ResetLevel()
    {
        Debug.Log("Restart triggered!");
        clockTime = songPlayer.clip.length;
    }
}
