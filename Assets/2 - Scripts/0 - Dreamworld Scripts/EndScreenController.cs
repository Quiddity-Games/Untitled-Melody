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
    [SerializeField] private Image ScorePanel;

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

    [SerializeField] private HitRatingColorSet colorSet;
    /*
    [SerializeField] private Color perfectColor;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;
    */
    [SerializeField] private Material perfectTextMaterial;
    [SerializeField] private Material goodTextMaterial;
    [SerializeField] private Material badTextMaterial;

    private List<TMP_Text> TextObjectsThatHaveOutlines = new List<TMP_Text>();
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text scoreText;

    private List<TMP_Text> TextObjectsNoOutlines = new List<TMP_Text>();
    [SerializeField] private TMP_Text foundText;
    [SerializeField] private TMP_Text neededText;
    [SerializeField] private TMP_Text neededNumberText;
    [SerializeField] private TMP_Text totalInLevelText;
    [SerializeField] private TMP_Text totalInLevelNumberText;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private TMP_Text restartText;

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

        TextObjectsThatHaveOutlines.Add(headerText);
        TextObjectsThatHaveOutlines.Add(scoreText);

        TextObjectsNoOutlines.Add(foundText);
        TextObjectsNoOutlines.Add(neededText);
        TextObjectsNoOutlines.Add(neededNumberText);
        TextObjectsNoOutlines.Add(totalInLevelText);
        TextObjectsNoOutlines.Add(totalInLevelNumberText);
        TextObjectsNoOutlines.Add(continueText);
        TextObjectsNoOutlines.Add(restartText);
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
            //obtainedCollectableText.color = Color.yellow;

            foreach (TMP_Text t in TextObjectsThatHaveOutlines)
            {
                t.fontMaterial = badTextMaterial;
            }

            foreach (TMP_Text t in TextObjectsNoOutlines)
            {
                t.color = colorSet.MissTextColor;
            }

            ScorePanel.color = colorSet.MissBackgroundColor;
            memiImage.sprite = endSprites.badSprite;
            restartButton.gameObject.SetActive(true);

            audioSource.clip = levelFailedSound;
        }
        else if (obtained == max)
        {
            titleText.text = "PERFECT!";
            //obtainedCollectableText.color = Color.red;

            foreach (TMP_Text t in TextObjectsThatHaveOutlines)
            {
                t.fontMaterial = perfectTextMaterial;
            }

            foreach (TMP_Text t in TextObjectsNoOutlines)
            {
                t.color = colorSet.GreatTextColor;
            }

            ScorePanel.color = colorSet.GreatBackgroundColor;
            memiImage.sprite = endSprites.perfectSprite;
            continueButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            levelManager.SetCurrentLevel(3);

            audioSource.clip = collectedAllFragmentsSound;
        }
        else if (obtained >= required)
        {
            titleText.text = "Good Job!";
            //obtainedCollectableText.color = Color.black;

            foreach (TMP_Text t in TextObjectsThatHaveOutlines)
            {
                t.fontMaterial = goodTextMaterial;
            }

            foreach (TMP_Text t in TextObjectsNoOutlines)
            {
                t.color = colorSet.GoodTextColor;
            }

            ScorePanel.color = colorSet.GoodBackgroundColor;
            memiImage.sprite = endSprites.goodSprite;
            continueButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            levelManager.SetCurrentLevel(3);

            audioSource.clip = collectedEnoughFragmentsSound;
        }
        else if (obtained > required)
        {
            titleText.text = "PERFECT!";
            //obtainedCollectableText.color = Color.red;

            foreach (TMP_Text t in TextObjectsThatHaveOutlines)
            {
                t.fontMaterial = perfectTextMaterial;
            }

            foreach (TMP_Text t in TextObjectsNoOutlines)
            {
                t.color = colorSet.GreatTextColor;
            }

            ScorePanel.color = colorSet.GreatBackgroundColor;
            memiImage.sprite = endSprites.perfectSprite;
            continueButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
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
