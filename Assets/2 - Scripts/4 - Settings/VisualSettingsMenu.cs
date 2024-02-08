using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSettingsMenu : MonoBehaviour
{

    public VisualSettings visual;

    public Toggle WindowedToggle;
    public Toggle ShakeToggle;
    public Toggle AnimatedBackgroundToggle;
    public Slider ContrastSlider;
    // Start is called before the first frame update
     public void Start()
    {
        LoadSettings();
        ShakeToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        WindowedToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        AnimatedBackgroundToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        ContrastSlider.onValueChanged.AddListener(delegate{ApplySettings();});
    }
    public void LoadSettings()
    {
        WindowedToggle.isOn = visual.Windowed;
        ShakeToggle.isOn = visual.Shake;
        AnimatedBackgroundToggle.isOn = visual.AnimatedBackground;
        ContrastSlider.value = visual.Contrast;
    }

    public void ApplySettings()
    {
        visual.Windowed = WindowedToggle.isOn;
        visual.Shake = ShakeToggle.isOn;
        visual.AnimatedBackground = AnimatedBackgroundToggle.isOn;
        visual.Contrast = ContrastSlider.value;
        this.visual.ApplySettings();
        SaveSettings();
    }

      public void SaveSettings()
    {
        this.visual.SaveSettings();
    }
}
