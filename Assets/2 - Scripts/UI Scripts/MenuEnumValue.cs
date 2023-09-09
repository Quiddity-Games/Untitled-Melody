using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnumValue : MonoBehaviour
{
    [SerializeField] private MainMenuManager.Menu value;

    public MainMenuManager.Menu Value()
    {
        return value;
    }
}
