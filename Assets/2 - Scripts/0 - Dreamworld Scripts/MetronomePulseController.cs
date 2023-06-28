using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomePulseController : MonoBehaviour
{
    
    public NoteTracker _NoteTracker;

    public MetronomePulse metronomePulse;
    private float rhythmIndicatorTimer; //Timer specifically dedicated to the rhythm indicator, aka the "metronome bars" above the player's head

    private float twoBeatsLength;

    [SerializeField] private float maxRadius;
    

    public void Start()
    {
        _NoteTracker.onBeatTrigger += HandlePulse;
    }

    // Start is called before the first frame update
    void HandlePulse()
    {
        MetronomePulse pulse =  Instantiate(metronomePulse, transform);
        pulse.Init((_NoteTracker.GetTwoBeatsLength() / 2), maxRadius);
    }


}
