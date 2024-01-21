using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [SerializeField] private CollectableInfo info;
    
    [Serializable] 
    private struct EndScreenSprites
    {
        public Sprite perfectSprite;
        public Sprite goodSprite;
        public Sprite badSprite;
    }
    
    [SerializeField] private EndScreenSprites endSprites;

    private void Start()
    {
        EndScreenMenu.SetActive(false);
    }

    public void LoadEndScreen()
    {
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
     
        }
        else if (obtained == max)
        {
            titleText.text = "PERFECT!";
            obtainedCollectableText.color = Color.red;
            memiImage.sprite = endSprites.perfectSprite;
            continueButton.gameObject.SetActive(true);
        }
        else if (obtained >= required)
        {
            titleText.text = "Good Job!";
            obtainedCollectableText.color = Color.black;
            memiImage.sprite = endSprites.goodSprite;
            continueButton.gameObject.SetActive(true);
        }

        
        EndScreenMenu.SetActive(true);
    }

    public void HideLoadScreen()
    {
        EndScreenMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
}
