using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{


    private bool m_isPaused;
    // Start is called before the first frame update
    void Start()
    {
        m_isPaused = false;
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_PAUSE, TogglePause);
    }

    void TogglePause()
    {
        m_isPaused = !m_isPaused;
        DreamworldEventManager.Instance.CallBoolEvent(DreamworldBoolEventEnum.ISPAUSED, m_isPaused);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
