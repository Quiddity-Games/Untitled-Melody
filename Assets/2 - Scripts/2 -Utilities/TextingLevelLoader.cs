using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLevelLoader : LevelLoader
{

    [SerializeField] private DialogueController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller.Initialize(_levelData.GetCurrentLevel().stringProprties["conversation"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
