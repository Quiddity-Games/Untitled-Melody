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
    [SerializeField] private CollectionResetter resetter; // not being used
    [SerializeField] private EndScreenController endScreen;

    private List<Collectable> currentCollectables = new List<Collectable>();

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

        DreamworldEventManager.RegisterCollectable += () => numCollectables += 1;
    }

    private void OnEnable()
    {
        DreamworldEventManager.OnDeath += HandleDeath;
        DreamworldEventManager.OnCollect += HandleCollection;
        DreamworldEventManager.EnterCheckpoint += RecordCurrentCollection;
        DreamworldEventManager.ResetTempCollection += ResetCollection;
    }

    private void OnDestroy()
    {
        DreamworldEventManager.RegisterCollectable -= () => numCollectables += 1;
        DreamworldEventManager.OnDeath -= HandleDeath;
        DreamworldEventManager.OnCollect -= HandleCollection;
        DreamworldEventManager.EnterCheckpoint -= RecordCurrentCollection;
        DreamworldEventManager.ResetTempCollection -= ResetCollection;
    }

    public CollectableInfo GetCollectionStats()
    {
        return endInfo;
    }
    
    public void Start()
    {
        UpdateInfo();
    }

    void HandleCollection(Collectable collected)
    {
        currentCollectables.Add(collected);
        tempNumCollected++;
        UpdateInfo();
        sound.PlaySound();
        if (numCollected + tempNumCollected >= numCollectables)
        {
            RecordCurrentCollection();

            DreamworldEventManager.OnGameEnd?.Invoke();

            //DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.GAME_END);
            //DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.INPUT_PAUSE);
        }
    }

    private void ResetCollection()
    {
        foreach (Collectable collected in currentCollectables)
        {
            collected.ResetDisplay();
        }

        currentCollectables.Clear();
    }

    public void HandleDeath()
    {
        _ui.UpdateLostCount();
        tempNumCollected = 0;
        UpdateInfo();

        DreamworldEventManager.ResetTempCollection?.Invoke();
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
        currentCollectables.Clear();
        //resetter.ClearTemp();
        UpdateInfo();
    }

    // not being used
    public void ClearTemp()
    {
        tempNumCollected = 0;
        UpdateInfo();
    }
}
