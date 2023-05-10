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
    public VoidCallback onGoodTrigger;
    public VoidCallback onGreatTrigger;
    public VoidCallback onPerfectTrigger;

    public VoidCallback offBeatTrigger;

    
    public VoidCallback onTimeUpdate;

    public bool onBeat;

    [SerializeField] private float _timeTracker;


    public bool inRange = false;
    public bool inPerfectRange = false;
    public bool inGoodRange = false;
    public bool inGreatRange = false;

    [SerializeField] private float bottomGoodRange = 0;
    [SerializeField] private float bottomGreatRange = 0;
    [SerializeField] private float bottomPerfectRange = 0;
    [SerializeField] private float topGoodRange = 0;
    [SerializeField] private float topGreatRange = 0;
    [SerializeField] private float topPerfectRange = 0;
    
    
    public float bpm //"Beats per minute" of the song
    {
        set
        {
            totalTime = 0;
            twoBeatsLength = 60f / bpm;
            nextBeatTime = twoBeatsLength + (twoBeatsLength / 4f);    //Added to have the "matchable rhythm" land on the second and fourth beats of each measure
            _bpm = value;
            timeTracker = 0;
           

        }

        get
        {
            return _bpm;
        }
    }
    public float timeTracker
    {
        set
        {

            
            inPerfectRange = value > bottomPerfectRange && value < topPerfectRange;
            inGreatRange = value > bottomGreatRange && value < topGreatRange && !inPerfectRange;
            inGoodRange = value > bottomGoodRange && value < topGoodRange && !inGoodRange && !inPerfectRange;
            inRange = inGoodRange || inGreatRange || inPerfectRange;
            
            if (inRange != onBeat)
            {
                onBeat = !onBeat;

                if (onBeat)
                {
                    onBeatTrigger?.Invoke();
                    if (inPerfectRange)
                    {
                        onPerfectTrigger?.Invoke();
                    }
                    else if (inGreatRange)
                    {
                        onGreatTrigger?.Invoke();
                    }
                    else
                    {
                        onGoodTrigger?.Invoke();
                    }
                }
                else
                {
                    offBeatTrigger?.Invoke();
                }
            }
            
            _timeTracker = value;
            if(value > nextBeatTime + goodRange){
            nextBeatTime += twoBeatsLength ;
            bottomGoodRange = nextBeatTime - goodRange;
            bottomGreatRange = nextBeatTime - greatRange;
            bottomPerfectRange = nextBeatTime - perfectRange;
            topGoodRange = nextBeatTime + goodRange;
            topGreatRange = nextBeatTime + greatRange;
            topPerfectRange = nextBeatTime + perfectRange;
           }

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

    public float GetGoodRange()
    {
        return goodRange;
    }
    
    public float GetGreatRange()
    {
        return greatRange;
    }
    public bool startedLevelCountdown;   //Determines whether/not the player has started the level

    private float nextBeatTime; //The "location" in time of the next beat that the player can hit/miss, aka the "matchable rhythm" for the dash mechanic
    private float twoBeatsLength;   //The length in seconds between two beats in the song's tempo. (Important b/c the "matchable rhythm" for the dash mechanic occurs only once every *two* beats of the song's tempo)
    [SerializeField] private float goodRange;  //The amount of time in seconds that the player can be "early" or "late" in hitting the beat, but still have it count as a successful hit
    [SerializeField] private float greatRange;
    [SerializeField] private float perfectRange;

}
