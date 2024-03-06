using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class CollectionScoreController : MonoBehaviour
{
    public static CollectionScoreController Instance;

    // Start is called before the first frame update
    
    public int numCollectables = 0; //Total number of collectables in the level
    [SerializeField] private int numCollected;    //Number of collectables the player has acquired so far
    [SerializeField] private int tempNumCollected; //"Temporary" information about collectables the player has claimed since their last checkpoint; used to determine what collectables they should lose / that should be reset the next time the player dies
    [SerializeField] private int requiredNumCollected;
    [SerializeField] private CollectionSound sound;
    [SerializeField] private CollectableUI _ui;
    [SerializeField] private CollectionResetter resetter;
    [SerializeField] private EndScreenController endScreen;

    public Action RegisterCollectable;

    public Sprite[] CollectableSprites;

    private CollectableInfo endInfo;
    void Awake()
    {
        endInfo = new CollectableInfo();
        Instance = this;

        numCollectables = 0;
        numCollected = 0;
        tempNumCollected = 0;
        endInfo.ResetValues();

        RegisterCollectable += () => numCollectables += 1;
    }

    public CollectableInfo GetCollectionStats()
    {
        return endInfo;
    }
    
    public void Start()
    {
        /*DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.REGISTER_COLLECTABLE, () => {
            numCollectables += 1;
            UpdateInfo();
        });*/

        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.DEATH, HandleDeath);
      
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.COLLECT, HandleCollection);

        DreamworldEventManager.Instance.RegisterVoidEventResponse(
            DreamworldVoidEventEnum.CHECKPOINT_ENTER, RecordCurrentCollection);
        UpdateInfo();
    }

    void HandleCollection()
    {
        tempNumCollected++;
        UpdateInfo();
        sound.PlaySound();
        if (numCollected + tempNumCollected >= numCollectables)
        {
            RecordCurrentCollection();
            DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.GAME_END);
            DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_PAUSE);
        }
    }

    public void HandleDeath()
    {
        _ui.UpdateLostCount();
        ClearTemp();
        resetter.ResetTempCollectables();
    }

    private void UpdateInfo()
    {
        endInfo.UpdateValues(numCollected + tempNumCollected, requiredNumCollected, numCollectables);
        _ui.UpdateUI(Math.Min(numCollected + tempNumCollected, numCollectables), numCollectables, tempNumCollected);
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
