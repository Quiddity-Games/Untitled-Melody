
using System;
using UnityEngine;

public class DashTracker : MonoBehaviour
{
    // Start is called before the first frame update
    public NoteTracker _NoteTracker;
    private PlayerControl _playerControl;

    private void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Enable();
    }

    private void Awake()
    {
        _NoteTracker.HitCallback += info => {MoveTracker();};
    }


    private void MoveTracker()
    {
        Vector2 mousePos = _playerControl.Dreamworld.MousePosition.ReadValue<Vector2>();
        Vector3 dashLocation =
            Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
        this.transform.position = dashLocation;
    }


}
