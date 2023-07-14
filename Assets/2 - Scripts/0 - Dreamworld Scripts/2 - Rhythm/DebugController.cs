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
        public int groupID;
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

    public List<Tuple<string, int>> GetOptionIdentifiers()
    {
        List<Tuple<string, int>> optionNames = new List<Tuple<string, int>>();

        foreach (DebugOptions opt in Options)
        {
            optionNames.Add(new Tuple<string, int>(opt.label, opt.groupID));
        }
        return optionNames;    
    }

    public void HandleOptions(string selectedOp, bool value)
    {
        
        foreach (DebugOptions opt in Options)
        {
            if (opt.label == selectedOp)
            {
                opt.callback.Invoke(value);
            }
 
        }
    }
        
}
