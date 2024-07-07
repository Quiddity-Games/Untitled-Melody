using System;
using UnityEngine;

/// <summary>
/// The script responsible for handling pause logic.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;

    public bool IsPaused;

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus)
        {
            OnPaused?.Invoke(true);
        }

    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {   
            OnPaused?.Invoke(pauseStatus);
        }
    }

    public static Action<bool> OnPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OnPaused += OnGamePause;
    }

    private void OnDestroy()
    {
        OnPaused -= OnGamePause;
    }

    private void Start()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    
    public void RegisterDreamworldEvents()
    {
        DreamworldEventManager.OnGameEnd += () => OnGamePause(true);
        DreamworldEventManager.OnDreamworldLeave += () => OnGamePause(false);
    }

    public void DeregisterDreamworldEvents()
    {
        DreamworldEventManager.OnGameEnd -= () => OnGamePause(true);
    }
    
    public void OnGamePause(bool isPaused)
    {
        IsPaused = isPaused;

        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}