using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class AudioMixerVariable : FloatVariable
{
    [SerializeField] private string mixerField;
    [SerializeField] private AudioMixer mixer;

    public override float Value 
    {
        
        set
        {
            mixer.SetFloat(mixerField, Mathf.Log10(value)*20);
            base.Value = value;
        }

        get
        {
            mixer.GetFloat(mixerField, out float returnValue);
            return Mathf.Pow(10, returnValue/20);
        }
    }
}
