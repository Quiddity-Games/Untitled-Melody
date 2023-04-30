using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

[CreateAssetMenu]
public class NoteTracker : ScriptableObject
{

    
    //Todo: Consider moving to a different class or use events?
    public bool isGameStart;

    private float _bpm;

    public VoidCallback onBeatTrigger;
    public VoidCallback offBeatTrigger;

    
    public VoidCallback onTimeUpdate;

    public bool onBeat;

    public float bpm //"Beats per minute" of the song
    {
        set
        {
            twoBeatsLength = 60f / bpm;
            nextBeatTime = twoBeatsLength + (twoBeatsLength / 4f);    //Added to have the "matchable rhythm" land on the second and fourth beats of each measure
            _bpm = value;
            timeTracker = 0;
            forgivenessRange = 0.4f;
        }

        get
        {
            return _bpm;
        }
    }

    [SerializeField] private float _timeTracker;


    public bool inRange = false;

    public float bottomRange;
    public float topRange;
    public float timeTracker
    {
        set
        {
            bottomRange = nextBeatTime - forgivenessRange;
            topRange = nextBeatTime + forgivenessRange;
            inRange = (value > (nextBeatTime - forgivenessRange) &&
                       value < (nextBeatTime + forgivenessRange));
            if (inRange != onBeat)
            {
                onBeat = !onBeat;

                if (onBeat)
                {
                    onBeatTrigger?.Invoke();
                }
                else
                {
                    offBeatTrigger?.Invoke();
                }
                
                
            }
            
            //Determines when the next beat of the matchable rhythm will be
            if(value > nextBeatTime + forgivenessRange)
            {
                nextBeatTime += twoBeatsLength;
            }
            
            _timeTracker = value;
            onTimeUpdate?.Invoke();
        }
        get
        {
            return _timeTracker;
        }
    }
    
    
    //How much time has passed since the player started the level; incremented with each frame
    public float startTime;    //The time at which the player started the level
    public float totalTime;

    public float GetNextBeatTime()
    {
        return nextBeatTime;
    }

    public float GetTwoBeatsLength()
    {
        return twoBeatsLength;
    }

    public float GetForgivenessRange()
    {
        return forgivenessRange;
    }
    
    public bool startedLevelCountdown;   //Determines whether/not the player has started the level

    private float nextBeatTime; //The "location" in time of the next beat that the player can hit/miss, aka the "matchable rhythm" for the dash mechanic
    private float twoBeatsLength;   //The length in seconds between two beats in the song's tempo. (Important b/c the "matchable rhythm" for the dash mechanic occurs only once every *two* beats of the song's tempo)
    private float forgivenessRange;  //The amount of time in seconds that the player can be "early" or "late" in hitting the beat, but still have it count as a successful hit

}
