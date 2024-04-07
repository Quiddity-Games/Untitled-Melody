using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlatformToggle : MonoBehaviour
{
    [SerializeField] private GameObject windowMenu;
    [SerializeField] private GameObject androidMenu;

    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            windowMenu.SetActive(true);
        }
        if(Application.platform == RuntimePlatform.Android)
        {
            androidMenu.SetActive(true);
        }
    }
}
