using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MetronomePulseController : MonoBehaviour
{
    
    public NoteTracker _NoteTracker;

    public MetronomePulse metronomePulse;
    private float rhythmIndicatorTimer; //Timer specifically dedicated to the rhythm indicator, aka the "metronome bars" above the player's head

    private float twoBeatsLength;

    [SerializeField] private float maxRadius;

    private float startRadius, endRadius;

    private PulseState pulseState;
    
    [SerializeField] private PulseState defaultState;
 
    public void SetState(PulseState state)
    {

        if (pulseState != PulseState.DISABLE)
        {
            _NoteTracker.onBeatTrigger -= HandlePulse;
        }
        
        if (state == PulseState.DISABLE)
        {
            _NoteTracker.onBeatTrigger -= HandlePulse;
        }
        else
        {
            _NoteTracker.onBeatTrigger += HandlePulse;
            if (state == PulseState.ONMAX || state == PulseState.ONEMIT)
            {
                startRadius = 0;
                endRadius = maxRadius;
            }
            else if (state == PulseState.ONMIN)
            {
                startRadius = maxRadius;
                endRadius = 5;
            }
        }
        
        
        Debug.Log(startRadius + ", " + maxRadius);
        pulseState = state;
        
    }

    public void Start()
    {
        SetState(defaultState);
    }

    // Start is called before the first frame update
    void HandlePulse()
    {
        if(pulseState == PulseState.ONEMIT){
        MetronomePulse pulse =  Instantiate(metronomePulse, transform);
        pulse.Init((_NoteTracker.GetTwoBeatsLength() / 2), startRadius, endRadius);
        }
        else
        {
            StartCoroutine(DelayPulse());
        }
    }

    IEnumerator DelayPulse()
    {
        yield return new WaitForSeconds(_NoteTracker.GetTwoBeatsLength() / 1.5f);
        MetronomePulse pulse =  Instantiate(metronomePulse, transform);
        pulse.Init((_NoteTracker.GetTwoBeatsLength() / 2), startRadius, endRadius);
    }

    public void EnableOnEmit(bool value)
    {
        if (value)
        {
            SetState(PulseState.ONEMIT);
        }
        else
        {
            if (pulseState == PulseState.ONEMIT)
            {
                SetState(PulseState.DISABLE);
            }
        }
    }

    public void EnableOnMax(bool value)
    {
        if (value)
        {
            SetState(PulseState.ONMAX);
        }
        else
        {
            if (pulseState == PulseState.ONMAX)
            {
                SetState(PulseState.DISABLE);
            }
        }
    }
    
    public void EnableOnMin(bool value)
    {
        if (value)
        {
            SetState(PulseState.ONMIN);
        }
        else
        {
            if (pulseState == PulseState.ONMIN)
            {
                SetState(PulseState.DISABLE);
            }
        }
    }

}
