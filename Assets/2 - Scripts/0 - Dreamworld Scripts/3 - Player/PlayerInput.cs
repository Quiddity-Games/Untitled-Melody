using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private PlayerControl _playerControl;

    [SerializeField] private BoolVariable PauseValue;

    private void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Pause.performed += PauseOnPerformed;
        _playerControl.Enable();
    }

    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
  
            PauseValue.Value = !PauseValue.Value;
          }

    private void OnDestroy()
    {
        _playerControl.Dreamworld.Pause.performed -= PauseOnPerformed;
    }


}
