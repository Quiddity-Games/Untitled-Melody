using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class CollectionScoreController : MonoBehaviour
{

    [SerializeField] private CheckpointSignal checkpointSignal;
    [SerializeField] private CollectionSignal signal;
    // Start is called before the first frame update
    
    private int numCollectables = 0; //Total number of collectables in the level
    [SerializeField] private int numCollected;    //Number of collectables the player has acquired so far
    [SerializeField] private int tempNumCollected; //"Temporary" information about collectables the player has claimed since their last checkpoint; used to determine what collectables they should lose / that should be reset the next time the player dies
    [SerializeField] private int requiredNumCollected;
        
    [SerializeField] private CollectableUI _ui;
    [SerializeField] private CollectionResetter resetter;
    [SerializeField] private EndScreenController endScreen;
    [SerializeField] private CollectableInfo endInfo;
    [SerializeField] private GameEvent onGameEnd;
    void Awake()
    {
        numCollectables = 0;
        numCollected = 0;
        tempNumCollected = 0;
        endInfo.ResetValues();
        signal.Register += () => { numCollectables += 1; };
        signal.SendCollect += HandleCollection;
        checkpointSignal.OnCheckpointEnter += value =>
        {
            RecordCurrentCollection();
        };
    }

    private void Start()
    {
        UpdateInfo();
    }

    void HandleCollection(Collectable collect)
    {
        UpdateCount();
        UpdateInfo();
        resetter.RegisterTemp(collect);
        if (numCollected + tempNumCollected >= numCollectables)
        {
            RecordCurrentCollection();
            onGameEnd.Raise();
        }
    }

    public void HandleDeath()
    {
        ClearTemp();
        resetter.ResetTempCollectables();
    }

    private void UpdateInfo()
    {
        endInfo.UpdateValues(numCollected + tempNumCollected, requiredNumCollected, numCollectables);
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
    }
    void UpdateCount()
    {
        tempNumCollected += 1;
    }

    public void RecordCurrentCollection()
    {
        numCollected += tempNumCollected;
        tempNumCollected = 0;
        resetter.ClearTemp();
        UpdateInfo();
    }

    public void ClearTemp()
    {
        tempNumCollected = 0;
        UpdateInfo();
    }
}
