using UnityEngine;

namespace Tools.Settings
{
    public class FloatSetting : ISetting<float>
    {
        private readonly float m_minValue;
        private readonly float m_maxValue;

        public FloatSetting(string name, float defaultValue, float minvalue = float.MinValue, float maxvalue = float.MaxValue) : base(name, defaultValue)
        {
            this.m_minValue = minvalue;
            this.m_maxValue = maxvalue;
        }

        protected override float InitializeValue(string name, float defaultValue)
        {
            return PlayerPrefs.GetFloat(m_settingName, defaultValue);   
        }

        public float GetMax() {return m_maxValue;}

        public float GetMin() {return m_minValue;}

        protected override float SetValue(float value)
        {
            value = Mathf.Clamp(value, m_minValue, m_maxValue);
            PlayerPrefs.SetFloat(m_settingName, value);
            return value;
        }
    }
}