using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Settings;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class SettingsSlider : MonoBehaviour
{

    private Slider m_slider;
    FloatSetting m_setting;

    float startVal;

    [SerializeField] AudioSource menuSFXSource;
    [SerializeField] AudioClip sliderAudioClip;

    public void Setup(FloatSetting setting)
    {
        m_setting = setting;
        m_slider.maxValue = setting.GetMax();
        m_slider.minValue = setting.GetMin();
        OnSettingChange(m_setting.Value);
        m_setting.OnValueChanged.AddListener(OnSettingChange);
    }

    private void Awake()
    {
        m_slider = GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(OnUIUpdate);
    }
    private void OnDestroy()
    {
        m_setting.OnValueChanged.RemoveListener(OnSettingChange);
    }

    private void OnSettingChange(float value)
    {
        m_slider.SetValueWithoutNotify(value);
    }

    public void OnUIUpdate(float value)
    {
        m_setting.Value = value;
    }

    public void OnStartSettingHandleDrag()
    {
        startVal = m_slider.value;
    }

    public void DuringHandleDrag()
    {
        if (m_slider.value >= startVal + 0.1f || m_slider.value <= startVal - 0.1f)
        {
            menuSFXSource.PlayOneShot(sliderAudioClip);
            startVal = m_slider.value;
        }
    }

}
