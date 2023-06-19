using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadController : MonoBehaviour
{
    
    private PlayerControl _playerControl;

    // Start is called before the first frame update
    void Start()
    {
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Reload.performed += context =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        };
        _playerControl.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
