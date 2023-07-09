using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugController : MonoBehaviour
{
    [Serializable]

    struct DebugOptions
    {
        public string label;
        public UnityEvent<bool> callback;
    }
    [SerializeField] private List<DebugOptions> Options;

    public List<string> GetDebugOptionsNames()
    {
        List<string> optionNames = new List<string>();

        foreach (DebugOptions opt in Options)
        {
            optionNames.Add(opt.label);
        }
        return optionNames;
    }

    public void HandleOptions(string selectedOp)
    {
        
        foreach (DebugOptions opt in Options)
        {
            if (opt.label != selectedOp)
            {
                opt.callback.Invoke(false);
            }
            else
            {
                opt.callback.Invoke(true);
            }
        }
    }
        
}
