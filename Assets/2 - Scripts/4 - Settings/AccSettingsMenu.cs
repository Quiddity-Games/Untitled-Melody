using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools.Settings;

public class AccSettingsMenu : BaseSubMenu
{

    public SettingsToggle SecondaryBarsToggle;
    public SettingsToggle MetronomeBlinkToggle;
    // Start is called before the first frame update
     public void Start()
    {
        SecondaryBarsToggle.Setup(Settings.SecondaryBars);
        MetronomeBlinkToggle.Setup(Settings.MetronomeBlink);
    }
   
}
