using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreBubbleUI : MonoBehaviour
{
    [SerializeField] NoteTracker noteTracker;

    [SerializeField] TextMeshProUGUI ratingText;
    [SerializeField] TextMeshProUGUI timingText;

    private Animator scoreBubbleAnimator;
    private Image scoreBubbleImage;
    private GameObject timingObject;

    private void Awake()
    {
        noteTracker.HitCallback += ShowScoreBubble;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreBubbleAnimator = GetComponent<Animator>();
        scoreBubbleImage = GetComponent<Image>();
        //originalPos = (transform as RectTransform).anchoredPosition;
        timingObject = timingText.gameObject;
    }


    void ShowScoreBubble(NoteTracker.HitInfo hitInfo)
    {
        //(transform as RectTransform).anchoredPosition = originalPos; // Set bubble to original position.

        // Reset the animation triggers on dash.
        scoreBubbleAnimator.SetTrigger("PopIn");

        // Set timing text. If PERFECT, do not display text.
        switch (hitInfo.timing)
        {
            case NoteTracker.BeatTiming.EARLY:
                timingText.text = "(Early)";
                timingObject.SetActive(true);
                break;
            case NoteTracker.BeatTiming.LATE:
                timingText.text = "(Late)";
                timingObject.SetActive(true);
                break;
            case NoteTracker.BeatTiming.PERFECT:
                break;
        }

        // Set rating text.
        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                ratingText.text = "Miss...";
                timingObject.SetActive(true);
                break;
            case NoteTracker.BeatRating.GOOD:
                ratingText.text = "Good!";
                timingObject.SetActive(false);
                break;
            case NoteTracker.BeatRating.GREAT:
                ratingText.text = "Great!!";
                timingObject.SetActive(false);
                break;
            case NoteTracker.BeatRating.PERFECT:
                ratingText.text = "Perfect!!!";
                timingObject.SetActive(false);
                break;
        }
    }
}
