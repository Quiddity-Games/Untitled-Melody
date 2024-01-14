using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualSettings", menuName = "ScriptableObjects/VisualSettings", order = 1)]
public class VisualSettings : ScriptableObject
{

    public bool Windowed;
    public bool Shake;
    public bool AnimatedBackground;
    public float Contrast;

    public OnSettingUpdate onUpdate;
   
   public void ApplySettings()
   {
        //TODO: ScreenShakeManager
        //TODO: AnimatedBackgroundManager
        Screen.fullScreen = !Windowed;
        onUpdate?.Invoke();
   }

   public void SaveSettings()
   {

    PlayerPrefs.SetInt("Windowed", Windowed ? 1 :0);
    PlayerPrefs.SetInt("Shake", Shake ? 1 :0);
    PlayerPrefs.SetInt("AnimatedBackground", AnimatedBackground ? 1 :0);
    PlayerPrefs.SetFloat("Contrast", this.Contrast);
    PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("Windowed"))
        {
        PlayerPrefs.SetInt("Windowed", 0);
        PlayerPrefs.Save();
        }
        this.Windowed = PlayerPrefs.GetInt("Windowed") != 0;

        if (!PlayerPrefs.HasKey("Shake"))
        {
            PlayerPrefs.SetInt("Shake",0);
            PlayerPrefs.Save();
        }
        this.Shake = PlayerPrefs.GetInt("Shake") != 0;

        if (!PlayerPrefs.HasKey("AnimatedBackground"))
        {
            PlayerPrefs.SetInt("AnimatedBackground", 1);
            PlayerPrefs.Save();
        }
        this.AnimatedBackground = PlayerPrefs.GetInt("AnimatedBackground") != 0;

        if (!PlayerPrefs.HasKey("Contrast"))
        {
            PlayerPrefs.SetFloat("Contrast", 0.0f);
            PlayerPrefs.Save();
        }
        this.Contrast = Mathf.Clamp(PlayerPrefs.GetFloat("Contrast"), 0.0f, 1f);
        this.ApplySettings();
    }
}
