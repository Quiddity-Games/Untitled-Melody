using UnityEngine;

namespace Tools.Settings
{
    public class IntSetting : ISetting<int>
    {
        private readonly int m_minValue;
        private readonly int m_maxValue;

        public IntSetting(string name, int defaultValue, int minvalue = int.MinValue, int maxvalue = int.MaxValue) : base(name, defaultValue) 
        {
            m_minValue = minvalue;
            m_maxValue = maxvalue;
        }

        protected override int InitializeValue(string name, int defaultValue)
        {
            return PlayerPrefs.GetInt(m_settingName, defaultValue);
        }

        protected override int SetValue(int value)
        {
            value = Mathf.Clamp(value, m_minValue, m_maxValue);
            PlayerPrefs.SetInt(m_settingName, value);
            return value;
        }
    }
}