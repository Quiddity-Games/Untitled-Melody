using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonDisplayManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseButton;
    
    // Start is called before the first frame update
    void Start()
    {
        DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.PAUSE, ToggleDisplay);
    }

    // Update is called once per frame
    void ToggleDisplay(bool isPaused)
    {
        
    }
}
