using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools.Settings;

[RequireComponent(typeof(Toggle))]
public class SettingsToggle : MonoBehaviour
{

    private Toggle m_toggle;
    BoolSetting m_setting;
    public void Setup(BoolSetting setting)
    {
        m_setting = setting;
        m_toggle.isOn = m_setting.Value;
        m_setting.OnValueChanged.AddListener(OnSettingChange);
    }

    private void OnEnable()
    {
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener(OnUIUpdate);
    }

    private void OnDestroy()
    {
        m_setting.OnValueChanged.RemoveListener(OnSettingChange);
    }

    public void OnSettingChange(bool value)
    {
        if(m_toggle == null)
        {
            m_toggle = GetComponent<Toggle>();
        }
        m_toggle.SetIsOnWithoutNotify(value);
    }

    public void OnUIUpdate(bool value)
    {
        if(m_setting != null)
        {
            m_setting.Value = value;
        }
    }
}
