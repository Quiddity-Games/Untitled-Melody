using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccSettingsMenu : MonoBehaviour
{
    public AccSettings accSettings;
    public Toggle ConstantToggle;
    public Toggle OccasionalToggle;
    public Toggle OffToggle;
    public Toggle SecondaryBarsToggle;
    public Toggle MetronomeBlinkToggle;
    // Start is called before the first frame update
     public void Start()
    {
        LoadSettings();
        ConstantToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        OccasionalToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        OffToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        SecondaryBarsToggle.onValueChanged.AddListener(delegate{ApplySettings();});
        MetronomeBlinkToggle.onValueChanged.AddListener(delegate{ApplySettings();});
    }
     public void LoadSettings()
    {
        DecodeMetronomeMode(accSettings.metronomeMode);
        SecondaryBarsToggle.isOn = accSettings.SecondaryBars;
        MetronomeBlinkToggle.isOn = accSettings.MetronomeBlink;
    }

    public MetronomeMode GetMetronomeMode(bool constant, bool occasional, bool off)
    {
        if(constant){return MetronomeMode.ALWAYS;}
        if(occasional){return MetronomeMode.OCCASIONAL;}
        else{return MetronomeMode.NONE;}
    }

    public void DecodeMetronomeMode(MetronomeMode mode)
    {
        switch(mode)
        {
            case MetronomeMode.ALWAYS:
                ConstantToggle.isOn = true;
                OccasionalToggle.isOn = false;
                OffToggle.isOn = false;
                break;
            case MetronomeMode.OCCASIONAL:
                ConstantToggle.isOn = false;
                OccasionalToggle.isOn = true;
                OffToggle.isOn = false;
                break;
            default:
                    ConstantToggle.isOn = false;
                    OccasionalToggle.isOn = false;
                    OffToggle.isOn = true;
                break;
        }
    }
    public void ApplySettings()
    {
        accSettings.metronomeMode = GetMetronomeMode(ConstantToggle.isOn, OccasionalToggle.isOn, OffToggle.isOn);

        accSettings.SecondaryBars = SecondaryBarsToggle.isOn;
        accSettings.MetronomeBlink = MetronomeBlinkToggle.isOn;
        accSettings.ApplySettings();
    }

      public void SaveSettings()
    {
        this.accSettings.SaveSettings();
    }
}
