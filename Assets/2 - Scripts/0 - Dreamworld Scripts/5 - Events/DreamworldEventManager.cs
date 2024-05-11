using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton Manager to Call Events for Dreamworld
/// </summary>
public class DreamworldEventManager : MonoBehaviour
{
    public static DreamworldEventManager Instance;

    public static Action OnDash;
    public static Action OnDeath;
    public static Action<Collectable> OnCollect;
    public static Action EnterCheckpoint;

    public static Action RegisterCollectable;
    public static Action ResetTempCollection;

    public static Action OnDialogueStart;
    public static Action OnDialogueContinue;
    public static Action OnDialogueEnd;

    public static Action OnCountdownFinish;
    public static Action OnGameStart;
    public static Action OnGameEnd;

    public static Action OnDreamworldLeave;
    public static Action OnReload;

    void OnDisable() { Instance = null; }

    public void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
        } 
    }

    private void Start()
    {
        if (InputManager.Instance)
            InputManager.Instance.RegisterDreamworldEvents();
        if (PauseMenuManager.Instance)
        {
            PauseMenuManager.Instance.RegisterDreamworldEvents();
            UIManager.SetPausePosition?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Instance)
            InputManager.Instance.DeregisterDreamworldEvents();
        if (PauseMenuManager.Instance)
            PauseMenuManager.Instance.DeregisterDreamworldEvents();
    }
}
