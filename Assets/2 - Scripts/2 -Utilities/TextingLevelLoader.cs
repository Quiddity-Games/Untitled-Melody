using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLevelLoader : LevelLoader
{
    public static TextingLevelLoader Instance;
    [SerializeField] private DialogueController _controller;
    public SceneManagerUtils _sceneManagerUtils;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller.Initialize(_levelData.GetCurrentLevel().stringProprties["conversation"]);
    }
}
