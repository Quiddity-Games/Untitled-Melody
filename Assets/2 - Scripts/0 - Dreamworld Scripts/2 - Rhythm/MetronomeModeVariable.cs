using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class MetronomeModeVariable : ScriptableObject
{
    public VoidCallback OnValueChange;
    [SerializeField] private MetronomeMode value;

    public MetronomeMode Value
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

    public void SetOccasional()
    {
        Value = MetronomeMode.OCCASIONAL;
    }
    
    public void SetNone()
    {
        Value = MetronomeMode.NONE;
    }
    
    public void SetAlways()
    {
        Value = MetronomeMode.ALWAYS;
    }
}
