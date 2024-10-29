using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TutorialPulseHandler : MonoBehaviour
{

    [SerializeField] private TutorialPulsePlacements[] pulseLocations;

    [SerializeField] private TutorialPulse pulsePrefab;

    [SerializeField] DebugPlatformObj debugPlatform;

    [Serializable] struct TutorialPulsePlacements
    {
        public Transform m_location;
        public string m_text;
        public string m_textMobile;
        
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
        if(tutorialPulse != null)
        {
            tutorialPulse.DeregisterOnCollide(AdvanceLocation);
        }
    }

    

    public void AdvanceLocation()
    {
        if(index < pulseLocations.Length)
        {
            tutorialPulse.transform.position = pulseLocations[index].m_location.position;

            bool isMobile = false;
            #if UNITY_EDITOR
                isMobile = debugPlatform.simulateMobile;
            #elif UNITY_STANDALONE
                isMobile = false;
            #elif UNITY_ANDROID || UNITY_IOS
                isMobile = true;
            #endif
            if(isMobile && pulseLocations[index].m_textMobile != "")
            {
                tutorialPulse.text.text =  pulseLocations[index].m_textMobile;    
            }
            else
            {
                tutorialPulse.text.text =  pulseLocations[index].m_text;
            }
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
