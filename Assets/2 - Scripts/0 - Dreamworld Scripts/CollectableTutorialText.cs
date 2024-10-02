using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the behavior of the tutorial text that appears at the start of the level, and determines when the player no longer needs this text. Attached to the DashTutorialText prefab.
/// </summary>
public class CollectableTutorialText : MonoBehaviour
{
    int numCollectable;  //Checks how many dashes the player has successfully pulled off so far

    private void OnEnable()
    {
        DreamworldEventManager.OnCollect += HandleTutorial;
    }

    private void OnDisable()
    {
        DreamworldEventManager.OnCollect -= HandleTutorial;
    }

    private void OnDestroy()
    {
        DreamworldEventManager.OnCollect -= HandleTutorial;
    }

    public void Start()
    {
        numCollectable = 0;
    }

    private void HandleTutorial(Collectable collected)
    {
        numCollectable++;
        if (numCollectable >= 3)
        {
            StartCoroutine(FadeAndDestroy());
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + (5 * Time.deltaTime), GetComponent<Transform>().position.z);
        }
    }

    /// <summary>
    /// A transition effect on the tutorial text to make the color change more "smooth"/gradual.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnToDefaultColor()
    {
        Color startColor = GetComponent<TMP_Text>().color;

        float t = 0f;

        while(t < 1)
        {
            Color lerpedColor = Color.Lerp(startColor, Color.white, t);
            GetComponent<TMP_Text>().color = lerpedColor;
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
        float alpha = GetComponent<TMP_Text>().color.a;

        while(alpha >= 0)
        {
            GetComponent<TMP_Text>().color = new Color(GetComponent<TMP_Text>().color.r, GetComponent<TMP_Text>().color.g, GetComponent<TMP_Text>().color.b, alpha);
            alpha -= 0.01f;

            yield return 0;
        }

        Destroy(gameObject);
    }
}
