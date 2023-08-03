using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBubbleUI : MonoBehaviour
{
    [SerializeField] NoteTracker noteTracker;

    [SerializeField] float offsetDuration;
    [SerializeField] float verticalOffset;
    [SerializeField] TextMeshProUGUI ratingText;
    [SerializeField] TextMeshProUGUI timingText;

    private Animator scoreBubbleAnimator;
    private Image scoreBubbleImage;
    private Vector2 originalPos;
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
        originalPos = (transform as RectTransform).anchoredPosition;
        timingObject = timingText.gameObject;
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

        StartCoroutine(FadeScoreBubble(offsetDuration, verticalOffset));
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
