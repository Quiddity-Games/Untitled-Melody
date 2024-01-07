using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private static PlayerControl _playerControl;

    private void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Pause.performed += PauseOnPerformed;
        _playerControl.Dreamworld.Dash.performed += DashOnPerformed;
        _playerControl.Enable();
    }

    public static void ToggleInput(bool value)
    {
        if (value)
        {
            Debug.Log("Enable Input");

            _playerControl.Enable();
        }
        else
        {
            Debug.Log("Disabled Input");

            _playerControl.Disable();
        }
    }
    private void DashOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Dash is being called");
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_DASH);
    }
    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_PAUSE);
    }

    public static Vector2 GetMousePosition()
    {
        return _playerControl.Dreamworld.MousePosition.ReadValue<Vector2>();
    }
    private void OnDestroy()
    {
        _playerControl.Dreamworld.Pause.performed -= PauseOnPerformed;
    }


}
