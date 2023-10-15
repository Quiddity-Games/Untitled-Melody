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
            Debug.Log("SET: " + value + ", " + Mathf.Log10(value)*20);
            mixer.SetFloat(mixerField, Mathf.Log10(value)*20);
            this.value = value;
            OnValueChange?.Invoke();        
        }

        get
        {
            mixer.GetFloat(mixerField, out value);
            Debug.Log("RESULT: " + Mathf.Pow(value/20,10) + ", " + value);
            return Mathf.Pow(10, value/20);
        }
    }
}
