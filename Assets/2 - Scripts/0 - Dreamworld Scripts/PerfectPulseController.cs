using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectPulseController : MonoBehaviour
{
    [SerializeField] private float pulseRadius;
    [SerializeField] private NoteTracker _noteTracker;

    [SerializeField] private float pulseForce;

    [SerializeField] private Transform origin;

    [SerializeField] private PerfectPulse pulsePrefab;

    private Coroutine pulseCoroutine;

    private float delay;

    private bool isEnabled;

    private int currentID = -1;

    [Serializable]
    struct PulseLocation
    {
        public int Id;
        public Transform location;
        public float delay;
    }

    [SerializeField] private List<PulseLocation> pulseArray;


    public void SetDelay(float seconds)
    {
        delay = seconds;
    }

    public void SetPosition(Transform location)
    {
        origin = location;
    }

    void Pulse(NoteTracker.HitInfo info)
    {
        if(info.rating == NoteTracker.BeatRating.PERFECT){
            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
            }
            
            if(currentID != -1){
                pulseCoroutine = StartCoroutine(InstantiatePulse());
            }
        }
    }

  
    public void AdjustPulseSetting(int Id)
    {
        if (currentID == Id)
        {
            currentID = -1;
            _noteTracker.HitCallback -= Pulse;
            return;
        }
      
        if(pulseArray.Count > Id)
        {
            _noteTracker.HitCallback += Pulse;
            PulseLocation location = pulseArray[Id];
            SetDelay(location.delay);
            SetPosition(location.location);
            currentID = Id;
        }
    }
    IEnumerator InstantiatePulse()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(pulsePrefab, origin.position,Quaternion.identity).Initialize(pulseRadius, pulseForce);
    }
}
