using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBubbleUI : MonoBehaviour
{
    [SerializeField] NoteTracker noteTracker;

    [SerializeField] float scoreBubbleOffsetDuration;
    [SerializeField] float scoreBubbleVerticalOffset;

    private Animator scoreBubbleAnimator;
    private TextMeshProUGUI scoreBubbleText;
    private Image scoreBubbleImage;
    private Vector2 originalPos;

    private void Awake()
    {
        noteTracker.HitCallback += ShowScoreBubble;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreBubbleAnimator = GetComponent<Animator>();
        scoreBubbleText = GetComponentInChildren<TextMeshProUGUI>();
        scoreBubbleImage = GetComponent<Image>();
        originalPos = (transform as RectTransform).anchoredPosition;
    }


    void ShowScoreBubble(NoteTracker.HitInfo hitInfo)
    {
        (transform as RectTransform).anchoredPosition = originalPos; // Set bubble to original position.

        // Reset the animation triggers on dash.
        scoreBubbleAnimator.ResetTrigger("FadeIn");
        scoreBubbleAnimator.ResetTrigger("FadeOut");

        // Reset the animation if currently animating on dash.
        if (scoreBubbleAnimator.GetBool("Fading"))
            scoreBubbleAnimator.SetBool("Fading", false);

        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                scoreBubbleText.text = "Miss...";
                break;
            case NoteTracker.BeatRating.BAD:
                scoreBubbleText.text = "Bad...";
                break;
            case NoteTracker.BeatRating.GOOD:
                scoreBubbleText.text = "Good!";
                break;
            case NoteTracker.BeatRating.GREAT:
                scoreBubbleText.text = "Great!!";
                break;
            case NoteTracker.BeatRating.PERFECT:
                scoreBubbleText.text = "Perfect!!!";
                break;
        }

        StartCoroutine(FadeScoreBubble(scoreBubbleOffsetDuration, scoreBubbleVerticalOffset));
    }

    IEnumerator FadeScoreBubble(float duration, float offsetY)
    {
        Vector2 currentPos = (transform as RectTransform).anchoredPosition;
        scoreBubbleAnimator.SetBool("Fading", true);
        scoreBubbleAnimator.SetTrigger("FadeIn");
        float time = 0f;

        while (time < duration)
        {
            (transform as RectTransform).anchoredPosition = Vector2.Lerp(currentPos, new Vector2(currentPos.x, currentPos.y + offsetY), time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        scoreBubbleAnimator.SetTrigger("FadeOut");
    }
}
