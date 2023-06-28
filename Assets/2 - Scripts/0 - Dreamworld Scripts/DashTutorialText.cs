using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the behavior of the tutorial text that appears at the start of the level, and determines when the player no longer needs this text. Attached to the DashTutorialText prefab.
/// </summary>
public class DashTutorialText : MonoBehaviour
{
    int numOfSuccessfulDashes;  //Checks how many dashes the player has successfully pulled off so far
    private NoteTracker _noteTracker;
    private CollectionScoreController _collectionScore;
    private VoidCallback onDestroy;

    public void Init(NoteTracker tracker, VoidCallback onDestroy)
    {
        numOfSuccessfulDashes = 0;
        this.onDestroy = onDestroy;
        _noteTracker = tracker;
        _noteTracker.HitCallback += HandleTutorial;
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
        if(_noteTracker != null)
        {
        _noteTracker.HitCallback -= HandleTutorial;
        }
    }

    private void HandleTutorial(NoteTracker.HitInfo info)
    {
        if (info.rating != NoteTracker.BeatRating.MISS)
        {
            numOfSuccessfulDashes++;
        }
        
        if (numOfSuccessfulDashes >= 3)
        {
            StartCoroutine(FadeAndDestroy());
            this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y + (5 * Time.deltaTime), this.GetComponent<Transform>().position.z);
        }
    }

    /// <summary>
    /// A transition effect on the tutorial text to make the color change more "smooth"/gradual.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnToDefaultColor()
    {
        Color startColor = this.GetComponent<TMP_Text>().color;

        float t = 0f;

        while(t < 1)
        {
            Color lerpedColor = Color.Lerp(startColor, Color.white, t);
            this.GetComponent<TMP_Text>().color = lerpedColor;
            t += Time.deltaTime;

            yield return 0;
        }
    }

    /// <summary>
    /// Fades out and removes the tutorial text when the player no longer needs it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAndDestroy()
    {
        float alpha = this.GetComponent<TMP_Text>().color.a;

        while(alpha >= 0)
        {
            this.GetComponent<TMP_Text>().color = new Color(this.GetComponent<TMP_Text>().color.r, this.GetComponent<TMP_Text>().color.g, this.GetComponent<TMP_Text>().color.b, alpha);
            alpha -= 0.01f;

            yield return 0;
        }

        Destroy(this.gameObject);
    }
}
