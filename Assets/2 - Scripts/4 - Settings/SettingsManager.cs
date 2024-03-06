using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{

    [SerializeField] private AccSettings accSettings;
    [SerializeField] private AVSettings aVSettings;
    [SerializeField] private VisualSettings visual;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject settingsPanel;

    private static SettingsManager _Instance;

    private void Awake()
    {
        if (_Instance != null && _Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            _Instance = this; 
        } 

        accSettings.ApplySettings();
        aVSettings.ApplySettings();
        visual.ApplySettings();
    }

    private void Start()
    {
        backButton.onClick.AddListener(TurnOffSettings);
        settingsPanel.SetActive(false);
    }

    public static SettingsManager Instance()
    {
        return _Instance;
    }
    public  VisualSettings GetVisualSettings()
    {
        return visual;
    }

    public  AccSettings GetAccSettings()
    {
        return accSettings;
    }
    
    public  AVSettings GetAudioSettings()
    {
        return aVSettings;
    }

    private void TurnOffSettings()
    {
        settingsPanel.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex > 0)
            PauseMenuManager.Instance.TogglePauseMenu(true);
    }
}
