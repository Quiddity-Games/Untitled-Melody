using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private ClickManager _clickManager;
    private PlayerControl _playerControl;

    private void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Enable();
    }

    void Update()
    {
        Vector2 mousePos = _playerControl.Dreamworld.MousePosition.ReadValue<Vector2>();
        _clickManager.CursorTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
    }
}
