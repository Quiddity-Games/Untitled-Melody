using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseSubMenu
{

    [SerializeField] LevelData data;
    [SerializeField] Button ContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        ContinueButton.gameObject.SetActive(PlayerPrefs.GetInt("Level", 1) != 0);
    }
}
