using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that allows for a button to call the static method to
// open the pause menu.
public class PauseButton : MonoBehaviour
{
    private Button m_button;

    void Awake()
    {
        m_button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_button.onClick.AddListener(() => PauseMenuManager.OnPaused?.Invoke(true));
    }

}
