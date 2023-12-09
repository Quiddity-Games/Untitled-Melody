using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private PlayerControl _playerControl;


    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private ClickManager _clickManager;

    private void Start()
    {
        DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.PAUSE, Pause);

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
        }
        else
        { 
            Time.timeScale = 1;
            playerBody.WakeUp();
        }
        
        _clickManager.ToggleControls(isPaused);
    }
}
