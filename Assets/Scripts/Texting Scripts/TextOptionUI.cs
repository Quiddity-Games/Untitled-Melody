using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOptionUI : MonoBehaviour
{
    public TextMeshProUGUI OptionText;
    public Button OptionButton;
    public int OptionIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (!OptionButton)
            OptionButton = GetComponent<Button>();
    }
}
