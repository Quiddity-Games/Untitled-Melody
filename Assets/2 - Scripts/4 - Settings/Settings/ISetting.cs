
using UnityEngine;
using UnityEngine.Events;
namespace Tools.Settings
{
    public abstract class ISetting<T>
    {
        protected readonly string m_settingName;

        private T m_value;
    
        public ISetting(string name, T defaultValue)
        {
            m_settingName = name;
            m_value = InitializeValue(name, defaultValue);
            OnValueChanged = new OnChangeEvent();
        }

        abstract protected T InitializeValue(string name, T defaultValue);

        public OnChangeEvent OnValueChanged { get; set; }

        public T Value{ get => m_value; 
        set
        {
            m_value = SetValue(value);
            OnValueChanged?.Invoke(value);
        }}

        abstract protected T SetValue(T value);

        public class OnChangeEvent : UnityEvent<T>
        {
        }
    }
}
