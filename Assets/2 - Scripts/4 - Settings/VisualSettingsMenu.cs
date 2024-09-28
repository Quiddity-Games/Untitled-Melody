using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSettingsMenu : BaseSubMenu
{

    public SettingsToggle WindowedToggle;
    public SettingsToggle ContrastToggle;
    public SettingsSlider ContrastSlider;
    // Start is called before the first frame update
     public void Start()
    {
        WindowedToggle.Setup(Settings.Windowed);
        ContrastToggle.Setup(Settings.ContrastEnabled);
        ContrastSlider.Setup(Settings.Contrast);
    }
    
}
