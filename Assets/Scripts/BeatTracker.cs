using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTracker : MonoBehaviour
{
    public static BeatTracker instance;
    public float timeTracker;
    public float beat;
    private float beatLength;
    public float beatRange;
    public bool onBeat;
    public GameObject beatVisual;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        beatLength = beat;
    }

    // Update is called once per frame
    void Update()
    {
        timeTracker += Time.deltaTime;
        if (timeTracker > (beat - beatRange) && timeTracker < (beat + beatRange))
        {
            onBeat = true;
        } else
        {
            onBeat = false;
        }
        if (onBeat)
        {
            beatVisual.SetActive(true);
        } else
        {
            beatVisual.SetActive(false);
        }

        if (timeTracker > beat+beatRange) {
            beat += beatLength;
        }
        
    }
}
