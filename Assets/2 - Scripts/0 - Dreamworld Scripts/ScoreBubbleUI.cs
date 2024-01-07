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
    [SerializeField] HitRatingColorSet hitRatingColors;

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
        timingObject = timingText.gameObject;
    }


    void ShowScoreBubble(NoteTracker.HitInfo hitInfo)
    {
        // Play the animation on dash.
        scoreBubbleAnimator.SetTrigger("PopIn");

        // Set timing text. If PERFECT, do not display text.
        switch (hitInfo.timing)
        {
            case NoteTracker.BeatTiming.EARLY:
                timingText.text = "(Early)";
                //timingObject.SetActive(true);
                break;
            case NoteTracker.BeatTiming.LATE:
                timingText.text = "(Late)";
                //timingObject.SetActive(true);
                break;
            case NoteTracker.BeatTiming.PERFECT:
                break;
        }

        // Set rating text.
        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                ratingText.text = "Miss...";
                ratingText.color = hitRatingColors.MissTextColor;
                scoreBubbleImage.color = hitRatingColors.MissBackgroundColor;
                timingText.color = hitRatingColors.MissTimingTextColor;
                timingObject.SetActive(true);
                break;
            case NoteTracker.BeatRating.GOOD:
                ratingText.text = "Good!";
                ratingText.color = hitRatingColors.GoodTextColor;
                scoreBubbleImage.color = hitRatingColors.GoodBackgroundColor;
                timingObject.SetActive(false);
                break;
            case NoteTracker.BeatRating.GREAT:
                ratingText.text = "Great!!";
                ratingText.color = hitRatingColors.GreatTextColor;
                scoreBubbleImage.color = hitRatingColors.GreatBackgroundColor;
                timingObject.SetActive(false);
                break;
            case NoteTracker.BeatRating.PERFECT:
                ratingText.text = "Perfect!!!";
                ratingText.color = hitRatingColors.PerfectTextColor;
                scoreBubbleImage.color = hitRatingColors.PerfectBackgroundColor;
                timingObject.SetActive(false);
                break;
        }
    }
}
