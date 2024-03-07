using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{

    public VoidCallback OnValueChange;
    [SerializeField] private string value;

    public void Set(string value)
    {
        this.value = value;
    }
    public string Value
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
