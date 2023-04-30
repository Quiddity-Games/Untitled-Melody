using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private PlayerControl _playerControl;

    [SerializeField] private GameEvent onPause;
    [SerializeField] private GameEvent onUnpause;

    [SerializeField] private BoolVariable pause;
    // Start is called before the first frame update
    void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Pause.performed += context =>
        {
            if(!pause.Value)
            {

                pause.Value = true;
                Time.timeScale = 0;
                onPause.Raise();

                //Unpausing
            }
            else
            {
                
                pause.Value = false;
                Time.timeScale = 1;
                onUnpause.Raise();

           
            }
        };
    }
}
