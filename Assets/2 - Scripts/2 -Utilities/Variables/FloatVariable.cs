using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public VoidCallback OnValueChange;
    private float value;

    public float Value
    {
        
        set
        {
            this.value = value;
            OnValueChange?.Invoke();        
        }

        get
        {
            return value;
        }
    }
    
}
