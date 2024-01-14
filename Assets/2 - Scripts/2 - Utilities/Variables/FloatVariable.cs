using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public VoidCallback OnValueChange;
    [SerializeField] private float value;

    public virtual float Value
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
