using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSettingsMenu : BaseSubMenu
{

    public List<SettingsToggle> WindowedToggle;
    public List<SettingsToggle> ContrastToggle;
    public List<SettingsSlider> ContrastSlider;
    // Start is called before the first frame update
     public void Start()
    {
        foreach(SettingsToggle toggle in  WindowedToggle)
        {
            toggle.Setup(Settings.Windowed);
        }
        foreach(SettingsToggle toggle in ContrastToggle)
        {
            toggle.Setup(Settings.ContrastEnabled);
        }
        foreach(SettingsSlider toggle in ContrastSlider)
        {
            toggle.Setup(Settings.Contrast);
        }
    }
    
}
