using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [Serializable]
    public struct Level
    {
        public string label;
        public int index;
        public LevelTypeEnum type;
        public int buildIndex;
        public int nextLevel;
        public StringIntDictionary properties;
        public StringStringDictionary stringProprties;
    }

    public List<Level> levels;

    [SerializeField] private int currLevelIndex;

    public void SetCurrentLevel(int index)
    {
        currLevelIndex = levels.Find(level => level.index == index).index;
    }
    
    public StringIntDictionary GetCurrentLevelProperties()
    {
        return levels[currLevelIndex].properties;
    }

    public int GetCurrentLevelIndex()
    {
        return currLevelIndex;
    }

    public Level GetCurrentLevel()
    {
        return levels[currLevelIndex];
    }

    public Level StepToNextLevel()
    {
        currLevelIndex = levels[currLevelIndex].nextLevel;
        return levels[currLevelIndex];
    }

    public Level GetNextLevel()
    {
        return levels[levels[currLevelIndex].nextLevel];
    }
}
