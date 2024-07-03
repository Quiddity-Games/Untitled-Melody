using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;


    private void Awake()
    {
        Settings.MusicVolume.OnValueChanged.AddListener(OnMusicVolumeChanged);
        Settings.SoundVolume.OnValueChanged.AddListener(OnSoundVolumeChanged);
        Settings.Windowed.OnValueChanged.AddListener(ToggleWindowed);
    }

    private void Start()
    {
        OnMusicVolumeChanged(Settings.MusicVolume.Value);
        OnSoundVolumeChanged(Settings.SoundVolume.Value);
        ToggleWindowed(Settings.Windowed.Value);
    }

    private void OnDestroy()
    {
        Settings.MusicVolume.OnValueChanged.RemoveListener(OnMusicVolumeChanged);
        Settings.SoundVolume.OnValueChanged.RemoveListener(OnSoundVolumeChanged);
        Settings.Windowed.OnValueChanged.RemoveListener(ToggleWindowed);
    }

    private void ToggleWindowed(bool value)
    {
      Screen.fullScreen = !value;
    }

    public void OnMusicVolumeChanged(float volume)
    {
      musicMixer.SetFloat("Volume", Mathf.Log10(volume)*20)
    }

    public void OnSoundVolumeChanged(float volume)
    {
      sfxMixer.SetFloat("Volume", Mathf.Log10(volume)*20);
    }

}
