using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScreenManager : BoolVariable
{
    public void ToggleFullScreen(bool value)
    {
        Screen.fullScreen = !value;
    }

    public override bool Value{
        set
        {
            Screen.fullScreen = value;
            OnValueChange?.Invoke();
        }

        get
        {
            return Screen.fullScreen;
        }
    }
    
}
