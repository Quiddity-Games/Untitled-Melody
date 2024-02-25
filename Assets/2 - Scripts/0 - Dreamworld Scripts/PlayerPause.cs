using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class PlayerPause : MonoBehaviour
{


    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private ClickManager _clickManager;

    private void Start()
    {
        DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.ISPAUSED, Pause);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void Pause(bool isPaused)
    {
        if(isPaused)
        { 
            Time.timeScale = 0;
            playerBody.Sleep();
            PlayerInput.ToggleInput(false);
        }
        else
        { 
            Time.timeScale = 1;
            playerBody.WakeUp();
            PlayerInput.ToggleInput(true);
        }
        
        _clickManager.ToggleControls(isPaused);
    }

    public void OnDisable()
    {
        Time.timeScale = 1;
    }
}
