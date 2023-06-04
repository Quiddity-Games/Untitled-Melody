using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private ClickManager _clickManager;
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

    void Update()
    {
        if(!PauseValue.Value){
        Vector2 mousePos = _playerControl.Dreamworld.MousePosition.ReadValue<Vector2>();
        _clickManager.CursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
        }
    }
}
