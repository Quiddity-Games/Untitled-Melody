using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer;
    public void ChangeVolume(float value)
    {
        mixer.SetFloat("musicVol", Mathf.Log10(value)*20);
    }
}
