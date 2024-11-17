using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerControl control;

    InputActionMap m_pausedActionMap;

    private void Awake()
    {
        Instance = this;
        control = new PlayerControl();
        control.Dreamworld.Dash.performed += OnDash;
        control.Dreamworld.Reload.performed += OnReload;
    }

    private void OnEnable()
    {
        PauseManager.OnPaused += ToggleInputOnPause;
    }

    private void OnDisable()
    {
        PauseManager.OnPaused -= ToggleInputOnPause;
    }

    public void RegisterDreamworldEvents()
    {
        DreamworldEventManager.OnCountdownFinish += () => ToggleDashAction(true);
        DreamworldEventManager.OnGameEnd += () => ToggleInputOnPause(true);
    }

    public void DeregisterDreamworldEvents()
    {
        DreamworldEventManager.OnCountdownFinish -= () => ToggleDashAction(true);
        DreamworldEventManager.OnGameEnd -= () => ToggleInputOnPause(true);
    }

    public void ToggleDashAction(bool on)
    {
        control.Dreamworld.Enable();

        if (on)
            control.Dreamworld.Dash.Enable();
        else
            control.Dreamworld.Dash.Disable();
    }

    private void Start()
    {
        EnableInput();

        if (SceneManager.GetActiveScene().buildIndex < 1)
            control.Dreamworld.Disable();

    }

    public void EnableInput()
    {
        control.Dreamworld.Enable();
    }

  

    public void DisableInput()
    {
        control.Dreamworld.Disable();
    }


    public void OnDash(InputAction.CallbackContext obj)
    {
        if (PauseManager.Instance.IsPaused)
            return;
        if(!obj.performed)
        {
            return;
        }
        DreamworldEventManager.OnDash?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext obj)
    {
        if (PauseManager.Instance.IsPaused)
            return;
        else
            PauseManager.OnPaused?.Invoke(false);

        DreamworldEventManager.OnReload?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext obj)
    {
        if(!obj.performed)
        {
            return;
        }
        PauseManager.OnPaused?.Invoke(!PauseManager.Instance.IsPaused);
    }

    private void ToggleInputOnPause(bool paused)
    {
        if (paused)
            DisableInput();
        else
            EnableInput();
    }

    public UnityEngine.Vector2 GetDirection()
    {
        return control.Dreamworld.Direction.ReadValue<UnityEngine.Vector2>();
    }

}
