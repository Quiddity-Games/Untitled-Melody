using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAsScrollbar : MonoBehaviour
{
    [SerializeField] ScrollRect rect;
    [SerializeField] Slider scrollSlider;

    // Update is called once per frame
    void Update()
    {
        rect.verticalNormalizedPosition = scrollSlider.value;
    }
}
