using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class SceneManagerUtils : ScriptableObject
{

    [SerializeField] private LevelData data;
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void InitializeLevel(int level)
    {
        SetCurrLevel(level);
        GoToLevel(level);
    }

    public void AdvanceLevel()
    {
        InitializeLevel(data.StepToNextLevel().buildIndex);
        
    }
    public void SetCurrLevel(int level)
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
    }

    public void LoadSavedLevel()
    {
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
