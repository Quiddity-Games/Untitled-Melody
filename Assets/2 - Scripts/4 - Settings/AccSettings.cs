using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccSettings", menuName = "ScriptableObjects/AccSettings", order = 1)]
public class AccSettings : ScriptableObject
{
    public bool enablePulse;
    public bool SecondaryBars;
    public bool MetronomeBlink;

    public OnSettingUpdate onUpdate;

    public void ApplySettings()
    {
        onUpdate?.Invoke();
    }

   public void SaveSettings()
   {

   }

   public void LoadSettings()
   {

   }
}
