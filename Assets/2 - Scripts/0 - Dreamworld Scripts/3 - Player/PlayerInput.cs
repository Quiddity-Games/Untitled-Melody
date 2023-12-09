using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private PlayerControl _playerControl;


    private void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Pause.performed += PauseOnPerformed;
        _playerControl.Dreamworld.Dash.performed += DashOnPerformed;
        _playerControl.Enable();
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_PAUSE, HandlePause);
    }

    public void HandlePause()
    {
        DreamworldEventManager.Instance.CallBoolEvent(DreamworldBoolEventEnum.PAUSE, true);
    }

    private void DashOnPerformed(InputAction.CallbackContext obj)
    {
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_DASH);
    }
    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_PAUSE);
    }

    private void OnDestroy()
    {
        _playerControl.Dreamworld.Pause.performed -= PauseOnPerformed;
    }


}
