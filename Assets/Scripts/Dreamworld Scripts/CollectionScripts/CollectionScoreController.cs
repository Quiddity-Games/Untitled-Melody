using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionScoreController : MonoBehaviour
{

    [SerializeField] private CollectionSignal signal;
    // Start is called before the first frame update
    
    private int numCollectables = 0; //Total number of collectables in the level
    private int numCollected;    //Number of collectables the player has acquired so far
    private int tempNumCollected; //"Temporary" information about collectables the player has claimed since their last checkpoint; used to determine what collectables they should lose / that should be reset the next time the player dies

    [SerializeField] private CollectableUI _ui;
    [SerializeField] private CollectionResetter resetter;
    void Awake()
    {
        numCollectables = 0;
        numCollected = 0;
        tempNumCollected = 0;
        signal.Register += () => { numCollectables += 1; };
        signal.SendCollect += HandleCollection;
    }

    void HandleCollection(Collectable collect)
    {
        UpdateCount();
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
        resetter.RegisterTemp(collect);
    }

    void HandleDeath()
    {
        ClearTemp();
        resetter.ClearTemp();
    }
    void UpdateCount()
    {
        tempNumCollected += 1;
    }

    public void RecordCurrentCollection()
    {
        numCollected += tempNumCollected;
        tempNumCollected = 0;
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
    }

    public void ClearTemp()
    {
        tempNumCollected = 0;
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables);
    }
}
