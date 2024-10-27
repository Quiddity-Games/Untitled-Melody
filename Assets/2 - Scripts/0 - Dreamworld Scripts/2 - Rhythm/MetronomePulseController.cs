using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MetronomePulseController : MonoBehaviour
{
    
    public NoteTracker _NoteTracker;

    [SerializeField] private bool keepOn;

    public MetronomePulse metronomePulse;
    private float rhythmIndicatorTimer; //Timer specifically dedicated to the rhythm indicator, aka the "metronome bars" above the player's head

    private float twoBeatsLength;

    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;

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
                endRadius = minRadius;
            }
        }
        
        
        pulseState = state;
        
    }

    public void Start()
    {
        if(keepOn)
        {
            SetState(PulseState.ONMIN);
        }
        else
        {
            SetState(PulseState.DISABLE);
            Settings.MetronomeRings.OnValueChanged.AddListener(TogglePulse);
        }
    }


    void OnDestroy()
    {
        SetState(PulseState.DISABLE);
        if(!keepOn)
        {
            Settings.MetronomeRings.OnValueChanged.RemoveListener(TogglePulse);
        }
    }

    private void TogglePulse(bool enabled)
    {
        if(enabled)
        {
            SetState(PulseState.ONMIN);
        }
        else
        {
            SetState(PulseState.DISABLE);
        }
    }

    void OnDisable()
    {
        _NoteTracker.onBeatTrigger -= HandlePulse;
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
