using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadController : MonoBehaviour
{
    void OnEnable()
    {
        //DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_RELOAD,
        //    ReloadScene
        //    );

        DreamworldEventManager.OnReload += ReloadScene;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void OnDisable()
    {
        //DreamworldEventManager.Instance.DeregisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_RELOAD,
        //    ReloadScene
        //);

        DreamworldEventManager.OnReload -= ReloadScene;
    }
}
