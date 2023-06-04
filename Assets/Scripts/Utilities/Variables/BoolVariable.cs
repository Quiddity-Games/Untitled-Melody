using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject
{

    public VoidCallback OnValueChange;
    [SerializeField] private bool value;

    public bool Value
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
