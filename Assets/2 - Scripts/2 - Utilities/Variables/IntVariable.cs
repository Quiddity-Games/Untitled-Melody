using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    public VoidCallback OnValueChange;
    private int value;

    public int Value
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
