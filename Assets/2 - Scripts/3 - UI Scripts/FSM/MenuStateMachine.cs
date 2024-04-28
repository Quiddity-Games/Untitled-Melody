using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateMachine : MonoBehaviour
{
    public MenuState currentState;

    public List<MenuState> states;

    public void Start()
    {
        currentState.onEnter?.Invoke();
    }

    public void SetState(MenuState menuValue)
    {
        if (menuValue != currentState)
        {
            currentState.onExit?.Invoke();
            currentState = menuValue;
            currentState.onEnter?.Invoke();
            return;
        }
    }

    public void StartStory()
    {
        GameManager.LoadNextScene("Dialogue Scene #1");
    }
}
