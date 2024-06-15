using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInitializer : MonoBehaviour
{
    [SerializeField] private FloatVariable _variable;

    [SerializeField] private Slider _slider;

    void Start()
    {
        _slider.value = _variable.Value;
        _slider.onValueChanged.AddListener(delegate { _variable.Value = _slider.value;});
    }
}
