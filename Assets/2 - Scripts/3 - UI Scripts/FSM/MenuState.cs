using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class MenuState : MonoBehaviour
{
    [SerializeField] private MenuEnum Value;

    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    public void RegisterEnterCallback(UnityAction callback)
    {
        onEnter.AddListener(callback);
    }
    
        
    public void RegisterExitCallback(UnityAction callback)
    {
        onEnter.AddListener(callback);
    }

    public void UnregisterCallback(UnityAction callback)
    {
        onEnter.RemoveListener(callback);
    }

    public MenuEnum GetState()
    {
        return Value;
    }
}
