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
    void Awake()
    {
        pause.Set(false);
        pause.OnValueChange += Pause;
    }

    public void Pause()
    {
        if(pause.Value)
        {
            
            Time.timeScale = 0;
            onPause.Raise();

            //Unpausing
        }
        else
        {
                
            Debug.Log("Unpause");
            Time.timeScale = 1;
            onUnpause.Raise();

           
        }
    }
}
