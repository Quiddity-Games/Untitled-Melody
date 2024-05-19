using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MemiCooldownDisplay : MonoBehaviour
{

    [SerializeField] private SpriteRenderer memiGlow;

    [SerializeField] private Color dashReadyColor;
    [SerializeField] private Color dashCooldownColor;
    // Start is called before the first frame update
    public void UpdateDisplay(bool isDashReady)
    {
        if(isDashReady)
        {
            memiGlow.color = dashReadyColor;
        }
        else
        {
            Debug.Log("Updated Color to cooldown");
            memiGlow.color = dashCooldownColor;
        }
    }
}
