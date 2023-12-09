using UnityEngine;

/// <summary>
/// The script responsible for pulling up the pause menu.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;

    private void Start()
    {
        DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.PAUSE, OnGamePause);
    }
    
    public void OnGamePause(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        pauseButton.SetActive(!isPaused);
    }
}