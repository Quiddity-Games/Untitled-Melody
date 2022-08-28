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
    bool startedLevel;
    public TMP_Text clock;
    public float clockTime;
    public Image clockBar;


    public float bpm;
    private float beatInterval, beatTimer;
    public static bool beatFull;
    public static int beatCountFull;

    public GameObject note;
    public AnimationCurve linearCurve;
    private GameObject myCanvas;

    // Start is called before the first frame update
    void Start()
    {
        songPlayer = GetComponent<AudioSource>();
        songPlayer.Stop();
        instance = this;
        beatLength = beat;
        beat -= .29411765f;
        //beat -= .147058825f;
        clockTime = (int)songPlayer.clip.length;
        myCanvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        //after starting the song by jumping, start clock and beat tracking
        if (startedLevel)
        {
            clockTime = songPlayer.clip.length - Time.time;
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
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Miss");
                }
                onBeat = false;
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
                //Debug.Log("gdsf");
                StartCoroutine(MetronomeNoteVisual(new Vector3(-405, -190, 0)));
                StartCoroutine(MetronomeNoteVisual(new Vector3(405, -190, 0)));
            }

            //set next beat
            if (timeTracker > beat + beatRange)
            {
                beat += beatLength;
            }
        } else
        {
            //Start Song
            if (Input.GetKeyDown(KeyCode.Space))
            {
                songPlayer.Play();
                songPlayer.volume = .5f;
                startedLevel = true;
            }
        }
        
    }

    void BeatDetection()
    {
        beatFull = false;
        beatInterval = 60 / bpm;
        beatTimer += Time.deltaTime;
        if (beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            beatFull = true;
            beatCountFull++;
        }
    }

    IEnumerator MetronomeNoteVisual(Vector3 startPos)
    {
        GameObject newNote = Instantiate(note, startPos, Quaternion.identity);
        newNote.GetComponent<RectTransform>().SetParent(myCanvas.transform, false);
        newNote.GetComponent<RectTransform>().anchoredPosition = startPos;
        Vector3 endPos = new Vector3(0, startPos.y,0);
        float t = 0;
        while (t < 1)
        {
            newNote.GetComponent<RectTransform>().anchoredPosition = Vector3.LerpUnclamped(startPos, endPos, linearCurve.Evaluate(t));
            t += Time.deltaTime * beatInterval;
            yield return 0;
        }
        Destroy(newNote);
        yield return 0;
    }
}
