using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDisableHover : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler 
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.gameObject.activeInHierarchy)
        {
            PlayerInputManager.ToggleInput(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        if(this.gameObject.activeInHierarchy) {
            PlayerInputManager.ToggleInput(true);
        }
    }
}
