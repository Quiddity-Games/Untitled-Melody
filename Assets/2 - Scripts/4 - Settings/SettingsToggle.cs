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

    bool m_isSetup = false;
    public void Setup(BoolSetting setting)
    {
        m_setting = setting;
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener(OnUIUpdate);
        OnSettingChange(m_setting.Value);
        m_setting.OnValueChanged.AddListener(OnSettingChange);
        m_isSetup = true;
    }

    private void OnDestroy()
    {
        if(m_isSetup)
        {
            m_setting.OnValueChanged.RemoveListener(OnSettingChange);
        }
    }

    public void OnSettingChange(bool value)
    {
        m_toggle.SetIsOnWithoutNotify(value);
    }

    public void OnUIUpdate(bool value)
    {
        m_setting.Value = value;
    }
}
