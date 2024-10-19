using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools.Settings;
public class AVSettingsMenu : BaseSubMenu
{
    public SettingsSlider bgm;
    public SettingsSlider sfx;

    public void Start()
    {
        bgm.Setup(Settings.MusicVolume);
        sfx.Setup(Settings.SoundVolume);
    }
}
