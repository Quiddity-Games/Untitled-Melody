using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInput _playerInput;

    InputActionMap m_pausedActionMap;

    private void Awake()
    {
        Instance = this;
        _playerInput = GetComponent<PlayerInput>();
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
        if (_playerInput.currentActionMap.name != "Dreamworld")
            _playerInput.SwitchCurrentActionMap("Dreamworld");

        if (on)
            _playerInput.currentActionMap.FindAction("Dash").Enable();
        else
            _playerInput.currentActionMap.FindAction("Dash").Disable();
    }

    private void Start()
    {
        EnableInput();

        if (SceneManager.GetActiveScene().buildIndex < 1 && _playerInput.currentActionMap.name == "Dreamworld")
            DisableInput();
    }

    public void EnableInput()
    {
        _playerInput.currentActionMap.Enable();
    }

    public void ReenableInput()
    {
        _playerInput.SwitchCurrentActionMap(m_pausedActionMap.name);
    }

    public void DisableInput()
    {
        m_pausedActionMap = _playerInput.currentActionMap;
        SwitchToUniversal();
    }

    public void SwitchToGameplay()
    {
        _playerInput.SwitchCurrentActionMap("Dreamworld");
    }

    public void SwitchToUniversal()
    {
        _playerInput.SwitchCurrentActionMap("Universal");
    }

    public void SwitchToUI()
    {
        _playerInput.SwitchCurrentActionMap("Texting");
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

    public void OnContinue(InputAction.CallbackContext obj)
    {
        if (DreamworldEventManager.Instance)
            DreamworldEventManager.OnDialogueContinue?.Invoke();
    }

    private void ToggleInputOnPause(bool paused)
    {
        if (paused)
            DisableInput();
        else
            ReenableInput();
    }

}
