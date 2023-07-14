using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectableUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentDisplay;
    [SerializeField] private TextMeshProUGUI maxDisplay;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI tempDisplay;
    [FormerlySerializedAs("lostColor")]
    [SerializeField] private Color bleedColor;

    private int tempCount;
    private CanvasGroup tempDisplayCanvasGroup;
    private GameObject tempDisplayObject;
    private Vector2 originalPos;

    private void Start()
    {
        tempDisplayCanvasGroup = tempDisplay.gameObject.GetComponent<CanvasGroup>();
        tempDisplayObject = tempDisplay.gameObject;
        originalPos = (tempDisplayObject.transform as RectTransform).anchoredPosition;
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
        ResetBleedDisplay();

        if (tempCount > 0)
        {
            int bleedCount = tempCount; // Snapshot the total bleed amount before it resets to 0.
            tempCount = 0; // Reset temp count on death.

            (tempDisplayObject.transform as RectTransform).anchoredPosition = originalPos;
            tempDisplay.text = "-" + bleedCount.ToString();
            tempDisplayObject.SetActive(true);

            currentDisplay.color = bleedColor;
            StartCoroutine(ShowFloatingText());
        }

        IEnumerator ShowFloatingText()
        {
            float time = 0f;
            float flashDuration = 1f;
            float fadeDuration = 2f;

            // Have current collectibles number flash red.
            while (time < flashDuration)
            {
                currentDisplay.color = Color.Lerp(bleedColor, Color.white, time / flashDuration);
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

    void ResetBleedDisplay()
    {
        currentDisplay.color = Color.white;

        (tempDisplayObject.transform as RectTransform).anchoredPosition = originalPos;
        tempDisplayCanvasGroup.alpha = 0f;
        tempDisplayObject.SetActive(true);
    }
}
