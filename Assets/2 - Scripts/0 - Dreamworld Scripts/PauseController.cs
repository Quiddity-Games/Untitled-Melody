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
    private void Awake()
    {
        pause.Set(false);
        pause.OnValueChange += Pause;
    }

    private void Start()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }

    public void Pause()
    {
        if(pause.Value)
        { 
            Time.timeScale = 0;
            onPause.Raise();
        }
        else
        { 
            Time.timeScale = 1;
            onUnpause.Raise();
        }
    }
}
