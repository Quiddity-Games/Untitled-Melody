using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI label;
    private VoidCallback_String callback;
    private string option;

    [SerializeField] private Toggle toggle;
    public void Initialize(string optionName, VoidCallback_String callback, ToggleGroup group)
    {
        option = optionName;
        label.text = optionName;
        this.callback = callback;
        toggle.group = group;
    }

    public void OnSelect(bool value)
    {
        if(value)
        {
            callback?.Invoke(option);
        }
    }
    
    
}
