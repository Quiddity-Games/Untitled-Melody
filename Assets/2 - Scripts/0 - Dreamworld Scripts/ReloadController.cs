using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadController : MonoBehaviour
{
    [SerializeField] private SceneManagerUtils manager;
    public void OnSceneLeave()
    {
        DreamworldEventManager.OnDreamworldLeave();
    }

    public void ReloadScene()
    {
        DreamworldEventManager.OnDreamworldLeave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

     public void Restart()
    {
        DreamworldEventManager.OnDreamworldLeave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevel(int level)
    {
        DreamworldEventManager.OnDreamworldLeave();
        manager.GoToLevel(level);
    }

    public void InitializeLevel(int level)
    {
        DreamworldEventManager.OnDreamworldLeave();
        manager.InitializeLevel(level);
    }

    public void AdvanceLevel()
    {
        DreamworldEventManager.OnDreamworldLeave();
        manager.AdvanceLevel();
    }
    public void SetCurrLevel(int level)
    {
        DreamworldEventManager.OnDreamworldLeave();
        manager.SetCurrLevel(level);
    }

    public void LoadSavedLevel()
    {
        DreamworldEventManager.OnDreamworldLeave();
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1));
    }

    public bool DoesSaveExist()
    {
        return PlayerPrefs.GetInt("Level", -1) != -1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
