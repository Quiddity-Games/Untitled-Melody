using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI display; 
    public void UpdateUI(int current, int total)
    {
        display.text =  "" + current + " / " + total;
    }
}
