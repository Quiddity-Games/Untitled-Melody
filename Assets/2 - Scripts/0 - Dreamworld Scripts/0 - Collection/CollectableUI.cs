using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentDisplay;
    [SerializeField] private TextMeshProUGUI maxDisplay;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI tempDisplay;
    [SerializeField] private Color lostColor;

    private int tempCount;
    private CanvasGroup tempDisplayCanvasGroup;
    private GameObject tempDisplayObject;

    private void Start()
    {
        tempDisplayCanvasGroup = tempDisplay.gameObject.GetComponent<CanvasGroup>();
        tempDisplayObject = tempDisplay.gameObject;
        tempDisplayObject.SetActive(false);
    }

    /// <summary>
    /// Updates the current text object, the maximum text object, and increases the number of temporarily collected collectibles.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="total"></param>
    /// <param name="temp"></param>
    public void UpdateUI(int current, int total, int temp)
    {
        currentDisplay.text = current.ToString();
        maxDisplay.text = total.ToString();
        tempCount = temp;
    }

    /// <summary>
    /// Handles the animation of the bleeding point UI.
    /// </summary>
    public void UpdateLostCount()
    {
        if (tempCount > 0)
        {
            int bleedCount = tempCount; // Snapshot the total bleed amount before it resets to 0.
            tempCount = 0; // Reset temp count on death.

            tempDisplay.text = "-" + bleedCount.ToString();
            tempDisplayObject.SetActive(true);

            currentDisplay.color = lostColor;
            StartCoroutine(ShowFloatingText());
        }

        IEnumerator ShowFloatingText()
        {
            Vector2 originalPos = (tempDisplayObject.transform as RectTransform).anchoredPosition;

            float time = 0f;
            float flashDuration = 1f;
            float fadeDuration = 2f;

            // Have current collectibles number flash red.
            while (time < flashDuration)
            {
                currentDisplay.color = Color.Lerp(lostColor, Color.white, time / flashDuration);
                time += Time.deltaTime;
                yield return null;
            }

            time = 0f;

            tempDisplayCanvasGroup.alpha = 1f;

            // Fade out bleed total and lerp downwards.
            while (time < fadeDuration)
            {
                tempDisplayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
                (tempDisplayObject.transform as RectTransform).anchoredPosition = Vector2.Lerp((tempDisplayObject.transform as RectTransform).anchoredPosition, new Vector2((tempDisplayObject.transform as RectTransform).anchoredPosition.x, (tempDisplayObject.transform as RectTransform).anchoredPosition.y - 0.5f), time / fadeDuration);
                time += Time.deltaTime;
                yield return null;
            }

            // Reset bleed number.
            (tempDisplayObject.transform as RectTransform).anchoredPosition = originalPos;
            tempDisplayObject.SetActive(false);
        }
    }
}
