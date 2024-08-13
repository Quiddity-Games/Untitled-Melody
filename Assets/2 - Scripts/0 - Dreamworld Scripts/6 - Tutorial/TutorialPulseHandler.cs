using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TutorialPulseHandler : MonoBehaviour
{

    [SerializeField] private TutorialPulsePlacements[] pulseLocations;

    [SerializeField] private TutorialPulse pulsePrefab;

    [Serializable] struct TutorialPulsePlacements
    {
        public Transform m_location;
        public string m_text;
    }

    private TutorialPulse tutorialPulse;

    [SerializeField] private NoteTracker _tracker;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        DreamworldEventManager.OnGameStart += Initialize;
    }

    void OnDestroy()
    {
        DreamworldEventManager.OnGameStart -= Initialize;
    }

    private void Initialize()
    {
        tutorialPulse = Instantiate(pulsePrefab, transform);
        index = 0;
        tutorialPulse.Initialize(_tracker);
        tutorialPulse.RegisterOnCollide(AdvanceLocation);
        AdvanceLocation();
    }

    void OnDisable()
    {
        tutorialPulse.DeregisterOnCollide(AdvanceLocation);
    }

    public void AdvanceLocation()
    {
        if(index < pulseLocations.Length)
        {
            tutorialPulse.transform.position = pulseLocations[index].m_location.position;
            tutorialPulse.text.text = pulseLocations[index].m_text;
            index++;
        }
        else
        {
            tutorialPulse.DeregisterOnCollide(AdvanceLocation);
            Destroy(tutorialPulse.gameObject);
            return;
        }
    }


}
