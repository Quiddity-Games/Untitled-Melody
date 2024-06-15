using System;
using System.Collections.Generic;
using UnityEngine;


public class NoteTracker : MonoBehaviour
{

    public SongObject _song;

    bool _isLoaded = false;

    void Start()
    {
        Load();
    }
    
    public void Load()
    {
        if(!_isLoaded)
        {
            totalTime = _song.song.length;
            twoBeatsLength = (60f / _song.bpm);
            nextBeatTime = twoBeatsLength;    //Added to have the "matchable rhythm" land on the second and fourth beats of each measure
            _bpm = _song.bpm;
            _timeTracker = 0;
            range.Enqueue(new BeatRange(nextBeatTime, perfectRange, BeatRating.PERFECT));
            range.Enqueue(new BeatRange(nextBeatTime, greatRange, BeatRating.GREAT));
            range.Enqueue(new BeatRange(nextBeatTime, goodRange, BeatRating.GOOD));
            _isLoaded = true;
        }
    }

    public enum BeatTiming
    {
        EARLY, LATE, PERFECT
    }
    public enum BeatRating
    {
        MISS, GOOD, GREAT, PERFECT
    }
    
    private float _bpm;

    public VoidCallback onLoad;
    public VoidCallback onBeatTrigger;
    public VoidCallback onBeatEnter;


    public VoidCallback onMissTrigger;

    public HitInfoCallback HitCallback;

    public VoidCallback offBeatTrigger;

    public VoidCallback onTimeUpdate;
    [SerializeField] private float _timeTracker;

    public bool onBeat;

    private bool inRange = false;
    private bool wasHit = false;
    [SerializeField] private bool actualBeat = false;


    [Serializable]
    private class BeatRangeQueue
    {
        private Queue<BeatRange> ranges;

        public BeatRangeQueue()
        {
            ranges = new Queue<BeatRange>();
        }
        public void Enqueue(BeatRange range)
        {
            ranges.Enqueue(range);
        }
        
        public void IncrementBeats(float newBeatTime)
        {
            foreach(BeatRange range in ranges)
            {
                range.IncrementRange(newBeatTime);
            }
        }

        public BeatRating PollRating(float timing)
        {
            foreach(BeatRange range in ranges)
            {
                if (range.InRange(timing))
                {
                    return range.GetRating();
                }
            }

            return BeatRating.MISS;
        }
    }

    private class BeatRange
    {
        private float _maxRange;
        private float _minRange;
        private float _range;

        private BeatRating _rating;
        public BeatRange(float firstBeat, float range, BeatRating rating)
        {
            _maxRange = firstBeat + range;
            _minRange = firstBeat - range;
            _range = range;
            _rating = rating;
        }
        public bool InRange(float time)
        {
            return (time > _minRange && time < _maxRange);
        }

        public BeatRating GetRating()
        {
            return _rating;
        }
        public void IncrementRange(float beatTime)
        {
            _maxRange = beatTime + _range;
            _minRange = beatTime - _range;
        }
    }

    private BeatRangeQueue range = new ();
    public float bpm //"Beats per minute" of the song
    {
        set
        {
            twoBeatsLength = (60f / value);
            nextBeatTime = twoBeatsLength;    //Added to have the "matchable rhythm" land on the second and fourth beats of each measure
            _bpm = value;
            _timeTracker = 0;
            range.Enqueue(new BeatRange(nextBeatTime, perfectRange, BeatRating.PERFECT));
            range.Enqueue(new BeatRange(nextBeatTime, greatRange, BeatRating.GREAT));
            range.Enqueue(new BeatRange(nextBeatTime, goodRange, BeatRating.GOOD));
       }

        get
        {
            return _bpm;
        }
    }

    public struct HitInfo
    {
        public BeatRating rating;
        public BeatTiming timing;

    }
    public void OnHit()
    {
        wasHit = true;

        HitInfo hitInfo = new HitInfo()
        {
            timing = _timeTracker > nextBeatTime ? BeatTiming.LATE : _timeTracker > nextBeatTime ? BeatTiming.EARLY : BeatTiming.PERFECT,
            rating = range.PollRating(_timeTracker)
        };
        
        HitCallback?.Invoke(hitInfo);
    }

    public float timeTracker
    {
        set
        {
            BeatRating rate = range.PollRating(_timeTracker);
            inRange = (rate != BeatRating.MISS);
      
            if (rate == BeatRating.PERFECT && !actualBeat)
            {
                actualBeat = true;
                onBeatTrigger?.Invoke();
            }
            
            if (inRange != onBeat)
            {
                
                onBeat = !onBeat;

                if (onBeat)
                {
               
                    onBeatEnter?.Invoke();
    
                }
                else
                {
                    if (!wasHit)
                    {
                        onMissTrigger?.Invoke();
                    }

                    actualBeat = false;
                    wasHit = false;
                    nextBeatTime += twoBeatsLength ;
                    range.IncrementBeats(nextBeatTime);
                    offBeatTrigger?.Invoke();
                }
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

    
    private float nextBeatTime; //The "location" in time of the next beat that the player can hit/miss, aka the "matchable rhythm" for the dash mechanic
    [SerializeField] private float twoBeatsLength;   //The length in seconds between two beats in the song's tempo. (Important b/c the "matchable rhythm" for the dash mechanic occurs only once every *two* beats of the song's tempo)
    [SerializeField] private float badRange;  
    [SerializeField] private float goodRange;  //The amount of time in seconds that the player can be "early" or "late" in hitting the beat, but still have it count as a successful hit
    [SerializeField] private float greatRange;
    [SerializeField] private float perfectRange;

}
