using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that allows for a button to call the static method to
// open the options menu.
public class OptionsButton : MonoBehaviour
{
    private Button m_button;
    [SerializeField] private GameObject m_origin;
    void Awake()
    {
        m_button = GetComponent<Button>();
    }
    void Start()
    {
        
        m_button.onClick.AddListener(() => {
            m_origin.SetActive(false);
            UIManager.OpenSettingsMenu(
                () => {
                    Debug.Log("Test");
                    m_origin.SetActive(true);
                    }
                );
    
            });
    }


}
