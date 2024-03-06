using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script responsible for pulling up the pause menu.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;

    public bool IsPaused;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    public static Action<bool> OnPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OnPaused += OnGamePause;
        OnPaused += TogglePauseMenu;
    }

    private void OnDestroy()
    {
        OnPaused -= OnGamePause;
        OnPaused -= TogglePauseMenu;
    }

    private void Start()
    {
        //DreamworldEventManager.Instance.RegisterBoolEventResponse(DreamworldBoolEventEnum.ISPAUSED, OnGamePause);
        pauseButton.onClick.AddListener(() => OnPaused?.Invoke(true));
        resumeButton.onClick.AddListener(() => OnPaused?.Invoke(false));

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void RegisterDreamworldEvents()
    {
        DreamworldEventManager.OnGameEnd += () => OnGamePause(true);
    }

    public void DeregisterDreamworldEvents()
    {
        DreamworldEventManager.OnGameEnd -= () => OnGamePause(true);
    }
    
    public void OnGamePause(bool isPaused)
    {
        pauseButton.gameObject.SetActive(!isPaused);
        IsPaused = isPaused;

        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void TogglePauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }
}