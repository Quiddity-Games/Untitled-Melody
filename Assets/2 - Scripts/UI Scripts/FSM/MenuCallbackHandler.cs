using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuCallbackHandler : MonoBehaviour
{
    [SerializeField] private MenuState state;
    [SerializeField] private UnityEvent callback;
    [SerializeField] private MenuTransitionEnum transition;
    public void Awake()
    {
        switch (transition)
        {
            case MenuTransitionEnum.ENTER:
                state.onEnter.AddListener(() => callback?.Invoke());
                break;
            case MenuTransitionEnum.EXIT:
                state.onExit.AddListener(() => callback?.Invoke());
                break;
        }
    }

}
