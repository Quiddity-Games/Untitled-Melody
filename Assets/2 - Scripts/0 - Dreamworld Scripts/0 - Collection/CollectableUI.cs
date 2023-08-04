using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CollectableUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentDisplay;
    [SerializeField] private TextMeshProUGUI maxDisplay;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI tempDisplay;
    [FormerlySerializedAs("lostColor")]
    [SerializeField] private Color bleedColor;

    private int tempCount;
    private RectTransform tempDisplayTransform;
    private CanvasGroup tempDisplayCanvasGroup;
    private GameObject tempDisplayObject;
    private Vector2 originalPos;

    private void Start()
    {
        tempDisplayCanvasGroup = tempDisplay.gameObject.GetComponent<CanvasGroup>();
        tempDisplayObject = tempDisplay.gameObject;
        tempDisplayTransform = (tempDisplayObject.transform as RectTransform);
        originalPos = tempDisplayTransform.anchoredPosition;
        tempDisplayObject.SetActive(false);

        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.gameObject.transform as RectTransform);
    }

    public void RestartCounter()
    {
        currentDisplay.text = "0";
        tempCount = 0;
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

            tempDisplayTransform.anchoredPosition = originalPos;
            tempDisplay.text = "-" + bleedCount.ToString();
            tempDisplayObject.SetActive(true);

            currentDisplay.color = bleedColor;
            StartCoroutine(ShowFloatingText());
        }

        IEnumerator ShowFloatingText()
        {
            float time = 0f;
            float duration = 2.5f;
            Vector2 endPos = new Vector2(originalPos.x, originalPos.y - 120f);

            // Have current collectibles number flash red. Fade out bleed total and lerp downwards.
            while (time < duration)
            {
                currentDisplay.color = Color.Lerp(bleedColor, Color.white, time / duration);
                tempDisplayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / duration);
                tempDisplayTransform.anchoredPosition = Vector2.Lerp(originalPos, endPos, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            tempDisplayCanvasGroup.alpha = 1f;

            // Reset bleed number.
            tempDisplayTransform.anchoredPosition = originalPos;
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
