using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialPulse : MonoBehaviour
{

    private Action onCollideWithPulse;

    [SerializeField] private MetronomePulseController controller;
    // Start is called before the first frame update

    public void Initialize(NoteTracker tracker)
    {
        controller._NoteTracker = tracker;
    }

    public void RegisterOnCollide(Action callback)
    {
        onCollideWithPulse += callback;
    }

    public void DeregisterOnCollide(Action callback)
    {
        onCollideWithPulse -= callback;
    }

 void OnTriggerEnter2D(Collider2D col)
    {
        onCollideWithPulse?.Invoke();
    }
}
