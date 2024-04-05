using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPulseHandler : MonoBehaviour
{

    [SerializeField] private Transform[] pulseLocations;

    [SerializeField] private TutorialPulse pulsePrefab;

    private TutorialPulse tutorialPulse;

    [SerializeField] private NoteTracker _tracker;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        tutorialPulse = Instantiate(pulsePrefab, transform);
        index = 0;
        tutorialPulse.Initialize(_tracker);
        tutorialPulse.RegisterOnCollide(AdvanceLocation);
    }

    void OnDisable()
    {
        tutorialPulse.DeregisterOnCollide(AdvanceLocation);
    }

    public void AdvanceLocation()
    {
        if(index < pulseLocations.Length)
        {
            tutorialPulse.transform.position = pulseLocations[index++].position;
        }
        else
        {
            tutorialPulse.DeregisterOnCollide(AdvanceLocation);
            Destroy(tutorialPulse.gameObject);
            return;
        }
    }


}
