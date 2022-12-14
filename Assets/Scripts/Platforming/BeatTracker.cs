using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BeatTracker : MonoBehaviour
{
    public static BeatTracker instance;
    public float timeTracker;
    public float beat;
    private float beatLength;
    public float beatRange;
    public bool onBeat;
    public GameObject beatVisual;
    public AudioSource songPlayer;
    public bool startedLevel;
    public TMP_Text clock;
    public float clockTime;
    public Image clockBar;
    public bool canClick;   //Determines if the player has "spent" their attempt to click/dash for the beat they're on

    public float bpm;
    private float beatInterval, beatTimer;
    public static bool beatFull;
    public static int beatCountFull;
    float startTime;
    public float beatTimerOffset;   //Specifies how long in seconds after the song begins that the first beat should take place (based on which "beats" of each measure -- 1, 2, 3, etc. -- the beat should land on)

    public GameObject note;
    public AnimationCurve linearCurve;
    private GameObject playerCanvas;

    public bool noteDebugMode;  //A debug tool to help the developer test the rhythm "forgiveness" value and see whether a pair of notes will count as a hit or not before clicking
    private Color noteColor;

    // Start is called before the first frame update
    void Start()
    {
        //startTime = Time.time;
        songPlayer = GetComponent<AudioSource>();
        songPlayer.Stop();
        songPlayer.volume = .5f;
        instance = this;
        beatLength = beat;
        beat += beatLength / 4f;    //Added to have the "beat" land on the second and fourth beats of each measure
        beatTimer = 0f - (beatTimerOffset) - 1f;
        //beatTimer += beatLength / 4f;
        //beat -= .29411765f;
        //beat -= .147058825f;
        clockTime = songPlayer.clip.length;
        playerCanvas = GameObject.Find("PlayerCanvas");
        canClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        //after starting the song by jumping, start clock and beat tracking
        if (startedLevel)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                clockTime = songPlayer.clip.length;
            }
            clockTime = songPlayer.clip.length - (Time.time - startTime);
            //Debug.Log(clockTime);
            clock.text = "" + clockTime;
            clockBar.fillAmount = clockTime/songPlayer.clip.length ;
            timeTracker += Time.deltaTime;

           // Check exact beats
            BeatDetection();

            //check if you click on beat within the beat range
            if (timeTracker > (beat - beatRange) && timeTracker < (beat + beatRange))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Hit");
                }
                onBeat = true;

                //Sets what color the note will be if the player clicks on this frame
                noteColor = Color.yellow;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Miss");
                }
                onBeat = false;

                noteColor = Color.red;
            }
            if (onBeat)
            {
                // beatVisual.SetActive(true);
            }
            else
            {
                //beatVisual.SetActive(false);
            }

            //Send Note on exact beat time
            if (beatFull)
            {
                StartCoroutine(MetronomeNoteVisual(new Vector3(-4f, 2, 0)));
                StartCoroutine(MetronomeNoteVisual(new Vector3(4f, 2, 0)));
            }

            //set next beat
            if (timeTracker > beat + beatRange)
            {
                beat += beatLength;
                canClick = true;    //Refreshes the player's attempt to click/dash
            }

        } else
        {
            //Start Song
            if (Input.GetKeyDown(KeyCode.Space))
            {
                songPlayer.Play();
                startTime = Time.time;
                startedLevel = true;
            }
        }
        
    }

    void BeatDetection()
    {
        beatFull = false;
        beatInterval = 60 / bpm;    //Space between each beat in seconds
        beatTimer += Time.deltaTime;
        //if(beatTimer >= beatInterval - (beatTimerOffset + beatTimerOffsetModifier))
        if(beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            beatFull = true;
            beatCountFull++;
        }
    }

    IEnumerator MetronomeNoteVisual(Vector3 startPos)
    {
        GameObject newNote = Instantiate(note, startPos, Quaternion.identity);
        newNote.GetComponent<RectTransform>().SetParent(playerCanvas.transform, false);
        newNote.GetComponent<RectTransform>().anchoredPosition = startPos;
        Vector3 endPos = new Vector3(0, startPos.y,0);
        bool instaDestroyNote = true;   //Used to determine whether/not a note should be instantly destroyed, or allowed to fade away in place for a moment (based on if player has clicked or not)
        float t = 0;
        while (t < 1)
        {
            newNote.GetComponent<RectTransform>().anchoredPosition = Vector3.LerpUnclamped(startPos, endPos, linearCurve.Evaluate(t));
            t += Time.deltaTime * beatInterval;
            //t += Time.deltaTime / beatInterval;

            //Changes note color _before_ player clicks, but only if debug mode is on
            if(noteDebugMode == true)
            {
                newNote.GetComponent<Image>().color = noteColor;
            }

            if(Input.GetMouseButtonDown(0))
            {
                newNote.GetComponent<Image>().color = noteColor;    //Changes the note's color after the player has clicked, so they can see whether/not they hit it
                t = 1;

                instaDestroyNote = false;
            }

            yield return 0;
        }

        //Either destroys the note, or causes it to fade away _then_ destroy, depending on whether or not the player "hits" it before it disappears
        if(instaDestroyNote == true)
        {
            Destroy(newNote);

        } else
        {
            float alpha = newNote.GetComponent<Image>().color.a;

            while(alpha >= 0)
            {
                newNote.GetComponent<Image>().color = new Color(newNote.GetComponent<Image>().color.r, newNote.GetComponent<Image>().color.g, newNote.GetComponent<Image>().color.b, alpha);
                alpha -= 0.02f; //Note to Self: Maybe for faster rhythms/songs, the alpha should fade faster, b/c more notes are coming in faster?

                yield return 0;
            }

            Destroy(newNote);
        }

        yield return 0;
    }
}
