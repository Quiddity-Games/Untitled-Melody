using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the loading/reloading of the level scene, primarily to allow the player to reset the level with the R key. Attached to the SceneManager gameObject.
/// </summary>
public class MySceneManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !Menus.paused)
        {
            GameManager.instance.numCollectables = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } 
    }
}
