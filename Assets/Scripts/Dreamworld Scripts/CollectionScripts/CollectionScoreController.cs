using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionScoreController : MonoBehaviour
{

    [SerializeField] private CheckpointSignal checkpointSignal;
    [SerializeField] private CollectionSignal signal;
    // Start is called before the first frame update
    
    private int numCollectables = 0; //Total number of collectables in the level
    [SerializeField] private int numCollected;    //Number of collectables the player has acquired so far
    [SerializeField] private int tempNumCollected; //"Temporary" information about collectables the player has claimed since their last checkpoint; used to determine what collectables they should lose / that should be reset the next time the player dies

    [SerializeField] private CollectableUI _ui;
    [SerializeField] private CollectionResetter resetter;
    void Awake()
    {
        numCollectables = 0;
        numCollected = 0;
        tempNumCollected = 0;
        signal.Register += () => { numCollectables += 1; };
        signal.SendCollect += HandleCollection;
        checkpointSignal.OnCheckpointEnter += value =>
        {
            RecordCurrentCollection();
        };
    }

    private void Start()
    {
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
    }

    void HandleCollection(Collectable collect)
    {
        UpdateCount();
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
        resetter.RegisterTemp(collect);
    }

    public void HandleDeath()
    {
        ClearTemp();
        resetter.ResetTempCollectables();
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
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
    }

    public void ClearTemp()
    {
        tempNumCollected = 0;
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
    }
}
