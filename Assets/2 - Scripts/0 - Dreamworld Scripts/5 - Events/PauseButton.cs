using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Button m_button;

    [SerializeField] private bool value;
    // Start is called before the first frame update
    void Start()
    {
        m_button.onClick.AddListener(Pause);
    }

    // Update is called once per frame
    void Pause()
    {
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_PAUSE);
    }

  
}
