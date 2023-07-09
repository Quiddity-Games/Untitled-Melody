using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugControllerRenderer : MonoBehaviour
{
    [SerializeField] private DebugController controller;

    [SerializeField] private ToggleController togglePrefab;

    [SerializeField] private ToggleGroup DebugMenu;
    private void Start()
    {
        InstantiateButtons();
    }

    private void InstantiateButtons()
    {
        List<string> optionsNames = controller.GetDebugOptionsNames();
        
        foreach(string option in optionsNames)
        {
            Instantiate(togglePrefab, DebugMenu.gameObject.transform).Initialize(option, controller.HandleOptions,DebugMenu);
        }
    }
}
