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
    

    // Start is called before the first frame update
    public void LoadEndScreen()
    {
        int required = info.requiredCollectables;
        int max = info.totalCollectables;
        int obtained = info.obtainedCollectables;
        neededCollectableText.text = max.ToString();
        obtainedCollectableText.text = obtained.ToString();
        totalCollectableText.text = max.ToString();
        if (obtained < required)
        {
            titleText.text = "Try Again!";
            obtainedCollectableText.color = Color.yellow;
            memiImage.sprite = endSprites.badSprite;
        }
        else if (obtained == required)
        {
            titleText.text = "Good Job!";
            obtainedCollectableText.color = Color.black;
            memiImage.sprite = endSprites.goodSprite;
        }
        else if (obtained > required)
        {
            titleText.text = "PERFECT!";
            obtainedCollectableText.color = Color.red;
            memiImage.sprite = endSprites.perfectSprite;
        }
        
        EndScreenMenu.SetActive(true);
    }

    public void HideLoadScreen()
    {
        EndScreenMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
