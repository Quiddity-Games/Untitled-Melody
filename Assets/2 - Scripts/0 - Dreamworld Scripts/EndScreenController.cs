using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI neededCollectableText;
    [SerializeField] private TextMeshProUGUI obtainedCollectableText;
    [SerializeField] private TextMeshProUGUI totalCollectableText;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image memiImage;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private GameObject EndScreenMenu;

    [SerializeField] private CollectionScoreController collectionScore;

    [SerializeField] private LevelData levelManager;
    
    [Serializable] 
    private struct EndScreenSprites
    {
        public Sprite perfectSprite;
        public Sprite goodSprite;
        public Sprite badSprite;
    }
    
    [SerializeField] private EndScreenSprites endSprites;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip collectedAllFragmentsSound;
    [SerializeField] private AudioClip collectedEnoughFragmentsSound;
    [SerializeField] private AudioClip levelFailedSound;

    private void OnEnable()
    {
        DreamworldEventManager.OnGameEnd += LoadEndScreen;
    }

    private void OnDestroy()
    {
        DreamworldEventManager.OnGameEnd -= LoadEndScreen;
    }

    private void Start()
    {
        EndScreenMenu.SetActive(false);
        //DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.GAME_END, LoadEndScreen);

        audioSource = this.GetComponent<AudioSource>();
    }
    
    public void LoadEndScreen()
    {
        CollectableInfo info = collectionScore.GetCollectionStats();
        int required = info.requiredCollectables;
        int max = info.totalCollectables;
        int obtained = info.obtainedCollectables;

        neededCollectableText.text = required.ToString();
        obtainedCollectableText.text = obtained.ToString();
        totalCollectableText.text = max.ToString();
        restartButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        if (obtained < required)
        {
            titleText.text = "Try Again!";
            obtainedCollectableText.color = Color.yellow;
            memiImage.sprite = endSprites.badSprite;
            restartButton.gameObject.SetActive(true);

            audioSource.clip = levelFailedSound;
        }
        else if (obtained == max)
        {
            titleText.text = "PERFECT!";
            obtainedCollectableText.color = Color.red;
            memiImage.sprite = endSprites.perfectSprite;
            continueButton.gameObject.SetActive(true);

            audioSource.clip = collectedAllFragmentsSound;
        }
        else if (obtained >= required)
        {
            titleText.text = "Good Job!";
            obtainedCollectableText.color = Color.black;
            memiImage.sprite = endSprites.goodSprite;
            continueButton.gameObject.SetActive(true);
            levelManager.SetCurrentLevel(3);

            audioSource.clip = collectedEnoughFragmentsSound;
        }
        else if (obtained > required)
        {
            titleText.text = "PERFECT!";
            obtainedCollectableText.color = Color.red;
            memiImage.sprite = endSprites.perfectSprite;
            continueButton.gameObject.SetActive(true);
            levelManager.SetCurrentLevel(3);

            audioSource.clip = collectedAllFragmentsSound;
        }
        
        EndScreenMenu.SetActive(true);

        audioSource.Play();
    }

    public void HideLoadScreen()
    {
        EndScreenMenu.SetActive(false);
    } 
}
