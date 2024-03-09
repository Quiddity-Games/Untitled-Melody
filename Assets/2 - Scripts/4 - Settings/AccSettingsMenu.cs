using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccSettingsMenu : BaseSubMenu
{
    public AccSettings accSettings;

    public Toggle SecondaryBarsToggle;
    public Toggle MetronomeBlinkToggle;
    // Start is called before the first frame update
     public void Start()
    {
        LoadSettings();

        SecondaryBarsToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        MetronomeBlinkToggle.onValueChanged.AddListener(delegate{ApplySettings();});
    }
     public void LoadSettings()
    {
        SecondaryBarsToggle.isOn = accSettings.SecondaryBars;
        MetronomeBlinkToggle.isOn = accSettings.MetronomeBlink;
    }

    public void ApplySettings()
    {
        accSettings.SecondaryBars = SecondaryBarsToggle.isOn;
        accSettings.MetronomeBlink = MetronomeBlinkToggle.isOn;
        accSettings.ApplySettings();
    }

      public void SaveSettings()
    {
        this.accSettings.SaveSettings();
    }
}
