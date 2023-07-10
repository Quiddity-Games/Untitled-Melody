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

    private Dictionary<int, ToggleGroup> groups;

    [SerializeField] private GameObject panel;
    private void Start()
    {
        groups = new Dictionary<int, ToggleGroup>();
        InstantiateButtons();
    }

    private void InstantiateButtons()
    {
        List<Tuple<string, int>> optionsNames = controller.GetOptionIdentifiers();
        
        foreach(Tuple<string, int> option in optionsNames)
        {
            ToggleGroup group;
            if (!groups.ContainsKey(option.Item2))
            {
                group = Instantiate(DebugMenu, this.transform);
                groups.Add(option.Item2, group);
            }
            else
            {
                group = groups[option.Item2];
            }
            
            Instantiate(togglePrefab, panel.transform).Initialize(option.Item1, controller.HandleOptions,group);
        }
    }
}
