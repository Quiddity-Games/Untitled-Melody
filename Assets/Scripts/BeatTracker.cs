using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public int clockTime;

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
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startedLevel)
        {
            clockTime = (int)songPlayer.clip.length - Mathf.FloorToInt(Time.time);
            clock.text = "" + clockTime;
            timeTracker += Time.deltaTime;
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

            if (timeTracker > beat + beatRange)
            {
                beat += beatLength;
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                songPlayer.Play();
                startedLevel = true;
            }
        }
        
    }
}
