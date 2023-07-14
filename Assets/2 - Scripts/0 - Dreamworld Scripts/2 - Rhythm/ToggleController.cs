using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI label;
    private VoidCallback_StringBool callback;
    private string option;

    [SerializeField] private Toggle toggle;
    public void Initialize(string optionName, VoidCallback_StringBool callback, ToggleGroup group)
    {
        option = optionName;
        label.text = optionName;
        this.callback = callback;
        toggle.group = group;
    }

    public void OnSelect(bool value)
    {
            callback?.Invoke(option, value);        
    }
    
    
}
