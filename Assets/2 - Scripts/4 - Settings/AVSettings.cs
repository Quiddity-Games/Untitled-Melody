using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AVSettings", menuName = "ScriptableObjects/AVSettings", order = 1)]
public class AVSettings : ScriptableObject
{

  [Range(0.0f, 1f)]
  public float musicVol = 1f;
  [Range(0.0f, 1f)]
  public float sfxVol = 1f;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioMixer musicMixer;

    public void ApplySettings()
    {
      musicMixer.SetFloat("musicVol", Mathf.Log10(musicVol)*20);
      sfxMixer.SetFloat("soundVol", Mathf.Log10(sfxVol)*20);
    }

    public void SaveSettings()
    {
    PlayerPrefs.SetFloat("musicVolume", this.musicVol);
    PlayerPrefs.SetFloat("sfxVolume", this.sfxVol);
    PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
    if (!PlayerPrefs.HasKey("musicVolume"))
    {
      PlayerPrefs.SetFloat("musicVolume", 0.8f);
      PlayerPrefs.Save();
    }
    this.musicVol = Mathf.Clamp(PlayerPrefs.GetFloat("musicVolume"), 0.0f, 1f);
    if (!PlayerPrefs.HasKey("sfxVolume"))
    {
      PlayerPrefs.SetFloat("sfxVolume", 0.8f);
      PlayerPrefs.Save();
    }
    this.sfxVol = Mathf.Clamp(PlayerPrefs.GetFloat("sfxVolume"), 0.0f, 1f);
    this.ApplySettings();
    }
}
