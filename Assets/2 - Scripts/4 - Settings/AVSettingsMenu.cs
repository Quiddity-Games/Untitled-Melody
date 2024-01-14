using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVSettingsMenu : MonoBehaviour
{

    public AVSettings avSettings;
    public Slider bgm;
    public Slider sfx;

    public void Start()
    {
        LoadSettings();
        bgm.onValueChanged.AddListener(delegate{ApplySettings();});
        sfx.onValueChanged.AddListener(delegate{ApplySettings();});
    }
    public void LoadSettings()
    {
        this.bgm.value = this.avSettings.musicVol;
        this.sfx.value = this.avSettings.sfxVol;
    }

    public void ApplySettings()
    {
        this.avSettings.musicVol = this.bgm.value;
        this.avSettings.sfxVol = this.sfx.value; 
        this.avSettings.ApplySettings();
    }

      public void SaveSettings()
    {
        this.avSettings.SaveSettings();
    }
}
