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

    private void Awake()
    {
        Instance = this;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        PauseMenuManager.OnPaused += ToggleInputOnPause;
    }

    private void OnDisable()
    {
        PauseMenuManager.OnPaused -= ToggleInputOnPause;
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

    public void DisableInput()
    {
        _playerInput.currentActionMap.Disable();
    }

    public void SwitchToGameplay()
    {
        _playerInput.SwitchCurrentActionMap("Dreamworld");
    }

    public void SwitchToUI()
    {
        _playerInput.SwitchCurrentActionMap("Texting");
        Debug.Log(_playerInput.currentActionMap);
    }

    public void OnDash(InputAction.CallbackContext obj)
    {
        if (PauseMenuManager.Instance.IsPaused)
            return;

        //DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_DASH);
        DreamworldEventManager.OnDash?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext obj)
    {
        if (PauseMenuManager.Instance.IsPaused)
            return;
        else
            PauseMenuManager.OnPaused?.Invoke(false);

        DreamworldEventManager.OnReload?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext obj)
    {
        PauseMenuManager.OnPaused?.Invoke(!PauseMenuManager.Instance.IsPaused);
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
            EnableInput();
    }

}
