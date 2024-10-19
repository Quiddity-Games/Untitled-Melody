using UnityEngine;

namespace Tools.Settings
{
    public class StringSetting : ISetting<string>
    {
        public StringSetting(string name, string defaultValue) : base(name, defaultValue) {}

        protected override string InitializeValue(string name, string defaultValue)
        {
            return PlayerPrefs.GetString(name, defaultValue);
        }

        protected override string SetValue(string value)
        {
            PlayerPrefs.SetString(m_settingName, value);
            return value;
        }
    }
}