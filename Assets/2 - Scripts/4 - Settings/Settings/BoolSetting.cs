using UnityEngine;

namespace Tools.Settings
{
    public class BoolSetting : ISetting<bool>
    {
        public BoolSetting(string name, bool defaultValue) : base(name, defaultValue){}

        protected override bool InitializeValue(string name, bool defaultValue)
        {
            return PlayerPrefs.GetInt(name, defaultValue ? 1 : 0) == 1;   
        }
        
        protected override bool SetValue(bool value)
        {
            PlayerPrefs.SetInt(m_settingName, value ? 1 : 0);
            return value;
        }
    }
}