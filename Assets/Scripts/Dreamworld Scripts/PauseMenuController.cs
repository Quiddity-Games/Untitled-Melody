using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
   private BoolVariable pauseValue;
   
   public void Pause()
   {
      pauseValue.Value = true;
   }

   public void TogglePause()
   {
      pauseValue.Value = !pauseValue.Value;
   }

  
}
