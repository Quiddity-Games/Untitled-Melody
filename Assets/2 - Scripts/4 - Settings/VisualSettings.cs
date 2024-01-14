using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualSettings", menuName = "ScriptableObjects/VisualSettings", order = 1)]
public class VisualSettings : ScriptableObject
{

    public bool Windowed;
    public bool Shake;
    public bool AnimatedBackground;
    public float Contrast;
   
   public void ApplySettings()
   {
    //TODO: ScreenShakeManager
    //TODO: AnimatedBackgroundManager
   }

   public void SaveSettings()
   {

   }

   public void LoadSettings()
   {

   }
}
