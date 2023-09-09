using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{

public enum Menu
{
    Story = 0,
    Stats = 1,
    Options = 2,
    Credits = 3,
    Exit = 4
}



private Dictionary<Menu, UnityEvent> menuCallbacks;

    public void RunMenu(MenuEnumValue value)
    {
        Menu enumVal = value.Value();
        if (menuCallbacks.ContainsKey(enumVal))
        {
            menuCallbacks[enumVal]?.Invoke();
        }
    }

    public void RegisterCallback(Menu menuEnum, UnityAction callback)
    {
        if (menuCallbacks.ContainsKey(menuEnum))
        {
            menuCallbacks[menuEnum].AddListener(callback);
            return;
        }
        menuCallbacks[menuEnum].AddListener(callback);
    }

    public void UnregisterCallback(Menu menuEnum, UnityAction callback)
    {
        if (menuCallbacks.ContainsKey(menuEnum))
        {
            menuCallbacks[menuEnum].RemoveListener(callback);
        }
    }
}
