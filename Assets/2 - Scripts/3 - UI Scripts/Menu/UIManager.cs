using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    public static Action SetPausePosition;

    private RectTransform pauseButtonTransform;
    private Vector2 initialOffsetMin;
    private Vector2 initialOffsetMax;

    [Header("Pause Menu")]
    [SerializeField] private BaseSubMenu pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [Header("Settings Menu")]
    [SerializeField] private BaseSubMenu settingsMenu;
    [Header("General Menu")]
    [SerializeField] private MenuNavigator m_navigator;
    public static UIManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        SetPausePosition += SetButtonPosition;
        PauseManager.OnPaused += TogglePauseMenu;
    }

    private void OnDestroy()
    {
        SetPausePosition -= SetButtonPosition;
        PauseManager.OnPaused -= TogglePauseMenu;
    }

    void Start()
    {
        pauseButton.onClick.AddListener(()=> {
            PauseManager.OnPaused?.Invoke(true);
        });
        pauseButtonTransform = pauseButton.GetComponent<RectTransform>();
        initialOffsetMin = pauseButtonTransform.offsetMin;
        initialOffsetMax = pauseButtonTransform.offsetMax;

        SetButtonPosition();
    }

    private void SetButtonPosition()
    {
        if (TextingLevelLoader.Instance)
        {
            pauseButtonTransform.offsetMax = new Vector2(-initialOffsetMin.x, -initialOffsetMin.y);
            pauseButtonTransform.offsetMin = new Vector2(-initialOffsetMax.x, -initialOffsetMax.y);
        } else
        {
            pauseButtonTransform.offsetMax = new Vector2(initialOffsetMax.x, initialOffsetMax.y);
            pauseButtonTransform.offsetMin = new Vector2(initialOffsetMin.x, initialOffsetMin.y);
        }

        if (SceneManager.GetActiveScene().buildIndex < 1)
            pauseButton.gameObject.SetActive(false);
        else
            pauseButton.gameObject.SetActive(true);
    }
    
    public void TogglePauseMenu(bool isPaused)
    {
        if(isPaused)
        {
            Instance.pauseButton.gameObject.SetActive(false);
            m_navigator.Reset();
            m_navigator.ChangeSubMenu(pauseMenu);
            Instance.m_navigator.onExitMenu += () => PauseManager.OnPaused?.Invoke(false);
        }
        else
        {
            Instance.pauseButton.gameObject.SetActive(true);
            m_navigator.Reset();
            Instance.m_navigator.onExitMenu = null;
            Instance.pauseButton.gameObject.SetActive(true);
        }
    }

    public static void OpenPauseMenu()
    {
        Instance.m_navigator.Reset();
        Instance.m_navigator.ChangeSubMenu(Instance.pauseMenu);
    }

    public static void OpenSettingsMenu(Action callback)
    {
        Instance.m_navigator.Reset();
        Instance.m_navigator.ChangeSubMenu(Instance.settingsMenu);
        Instance.m_navigator.onExitMenu += callback;
    }

    public void RegisterDreamworldEvents()
    {
        DreamworldEventManager.OnGameEnd += () => TogglePauseButton(false);
        DreamworldEventManager.OnDreamworldLeave += () => TogglePauseButton(true);
    }

    public void TogglePauseButton(bool value)
    {
        pauseButton.gameObject.SetActive(value);
    }

    public void DeregisterDreamworldEvents()
    {
        DreamworldEventManager.OnGameEnd -= () => TogglePauseButton(false);
        DreamworldEventManager.OnDreamworldLeave -= () => TogglePauseButton(true);    
    }
}
