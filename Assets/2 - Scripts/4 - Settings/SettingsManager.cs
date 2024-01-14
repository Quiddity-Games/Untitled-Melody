using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    [SerializeField] private AccSettings accSettings;
    [SerializeField] private AVSettings aVSettings;
    [SerializeField] private VisualSettings visual;

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
}
