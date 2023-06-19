using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// The script that handles all of the branching dialogue / "Texting" functionality using the Inkle plugin. Attached to the Dialogue UI Canvas gameObject.
/// </summary>
[Serializable] 
public class DialogueCanvasUI : MonoBehaviour
{
    [SerializeField] Button startDialogueButton;
    public Button ContinueDialogueButton;

    [Header("Phone UI: Overall")]
    [FormerlySerializedAs("cellPhoneCanvasGroup")]
    [SerializeField] CanvasGroup phoneContainerCanvasGroup;

    [Header("Phone UI: Header")]
    [SerializeField] CanvasGroup headerCanvasGroup;
    [SerializeField] Image headerIcon;
    [SerializeField] TextMeshProUGUI headerText;

    [Header("Phone UI: Body")]
    [SerializeField] RectTransform bodyScrollViewTransform;
    [SerializeField] GameObject bodyScrollContent;
    [SerializeField] ScrollRect bodyScrollRect;
    [SerializeField] GameObject textOptionsContainer;

    #region Hidden Variables
    [HideInInspector] float textContainerTop;
    [HideInInspector] float textContainerRight;
    [HideInInspector] float textContainerLeft;
    [HideInInspector] float textContainerBottom;
    #endregion

    void Start()
    {
        // Set up UI layout and visibility.
        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.gameObject.transform as RectTransform);
        phoneContainerCanvasGroup.alpha = 0f;
        headerCanvasGroup.alpha = 0f;

        // Give functions to buttons.
        startDialogueButton.onClick.AddListener(ShowDialogueUI);
        ContinueDialogueButton.onClick.AddListener(PlayDialogue);

        // Get current size of texting body container.
        textContainerLeft = bodyScrollViewTransform.offsetMin.x;
        textContainerRight = bodyScrollViewTransform.offsetMax.x;
        textContainerTop = bodyScrollViewTransform.offsetMax.y;
        textContainerBottom = bodyScrollViewTransform.offsetMin.y;
    }

    /// <summary>
    /// Show dialogue UI after delay on start.
    /// </summary>
    void ShowDialogueUI()
    {
        DialogueController.Instance.FadeInUI(phoneContainerCanvasGroup, DialogueController.Instance.CanvasFadeDuration);
        GetHeaderText();
        startDialogueButton.gameObject.SetActive(false);
        ContinueDialogueButton.interactable = false;

        StartCoroutine(PlayFirstLine(DialogueController.Instance.CanvasFadeDuration));

        IEnumerator PlayFirstLine(float duration)
        {
            float time = 0;
            duration += DialogueController.Instance.StartDelayDuration;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            PlayDialogue();
            ContinueDialogueButton.interactable = true;
        }
    }

    /// <summary>
    /// Create a text bubble using the parsed text.
    /// </summary>
    /// <param name="line"></param>
    public void CreateTextBubble(string line)
    {
        GameObject textBubble = Instantiate(DialogueController.Instance.TextBubblePrefab, bodyScrollContent.transform);
        textBubble.SetActive(false);
        DialogueController.Instance.BubblesBeforeChoice.Add(textBubble.GetComponent<TextBubbleUI>());
        
        TextBubbleUI ui = textBubble.GetComponent<TextBubbleUI>();
        string speakerName = ui.ParseSpeaker(line);
        DialogueController.Instance.FadeInUI(ui.CanvasGroup, DialogueController.Instance.BubbleFadeDuration);

        ui.SetTextBubbleInformation(line, TextBubbleCharacterUI.Instance.MainCharacterName, DialogueController.Instance.CharacterUIDictionary[speakerName]);
    }

    /// <summary>
    /// Set the header text and icon.
    /// Can use an alias with tag "#Alias: x", if applicable (not currently in use).
    /// </summary>
    void GetHeaderText()
    {
        headerIcon.sprite = DialogueController.Instance.CharacterUIDictionary[DialogueController.Instance.GlobalTagsDictionary["Conversation"]].IconSprite;
        headerText.text = DialogueController.Instance.GlobalTagsDictionary["Conversation"];

        LayoutRebuilder.ForceRebuildLayoutImmediate(headerCanvasGroup.gameObject.transform as RectTransform);
        DialogueController.Instance.FadeInUI(headerCanvasGroup, DialogueController.Instance.BubbleFadeDuration);
    }

    /// <summary>
    /// Displays all current instantiated bubbles in order until dialogue ends or choices appear.
    /// </summary>
    public void PlayDialogue()
    {
        List<TextBubbleUI> bubblesBeforeChoice = DialogueController.Instance.BubblesBeforeChoice;
        int currentBubbleIndex = DialogueController.Instance.CurrentBubbleIndex;

        if (bubblesBeforeChoice.Count > 0 && !bubblesBeforeChoice[bubblesBeforeChoice.Count - 1].gameObject.activeInHierarchy)
        {
            ContinueDialogueButton.interactable = true;

            // Check the current index of the shown bubble, and set it to true.
            bubblesBeforeChoice[currentBubbleIndex].gameObject.SetActive(true);
            DialogueController.Instance.FadeInUI(bubblesBeforeChoice[currentBubbleIndex].CanvasGroup, DialogueController.Instance.BubbleFadeDuration);
            bodyScrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom of the container.

            DialogueController.Instance.CurrentBubbleIndex++;
        }
        else if (bubblesBeforeChoice[bubblesBeforeChoice.Count - 1].gameObject.activeInHierarchy)
        {
            ContinueDialogueButton.interactable = false;
            DialogueController.Instance.LinesBeforeChoice.Clear();
            bubblesBeforeChoice.Clear();

            DialogueController.Instance.CurrentBubbleIndex = 0;

            // If there are choices to be shown, show them.
            if (DialogueController.Instance.InkStory.currentChoices.Count > 0)
                DisplayChoices();
        }
    }

    /// <summary>
    /// Creates a container for options and instantiates a number of option buttons based on Ink.
    /// </summary>
    void DisplayChoices()
    {
        List<TextOptionUI> currentOptions = DialogueController.Instance.CurrentOptions;

        ContinueDialogueButton.gameObject.SetActive(false);
        
        for (int i = 0; i < DialogueController.Instance.InkStory.currentChoices.Count; i++)
        {
            GameObject optionButton = Instantiate(DialogueController.Instance.OptionButtonPrefab, textOptionsContainer.transform);
            TextOptionUI ui = optionButton.GetComponent<TextOptionUI>();
            ui.OptionFontColour = DialogueController.Instance.CharacterUIDictionary[TextBubbleCharacterUI.Instance.MainCharacterName].FontColor;

            currentOptions.Add(ui);

            ui.OptionText.color = ui.OptionFontColour;
            ui.OptionText.text = DialogueController.Instance.InkStory.currentChoices[i].text;
            ui.OptionIndex = i;
        }

        // Set event to buttons and fade them in gradually.
        foreach(TextOptionUI options in currentOptions)
        {
            options.OptionButton.onClick.AddListener(() => DialogueController.Instance.ChoiceMadeCallback(options.OptionIndex));
            DialogueController.Instance.FadeInUI(options.GetComponent<CanvasGroup>(), DialogueController.Instance.BubbleFadeDuration + options.OptionIndex * 0.15f);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.transform as RectTransform);

        bodyScrollViewTransform.offsetMin = new Vector2(textContainerLeft, (textOptionsContainer.transform as RectTransform).sizeDelta.y);

    }

    /// <summary>
    /// Reset main text container size values after making a choice.
    /// </summary>
    public void ResetTextContainerSize()
    {
        // Reset container size values.
        TextOptionUI topOption = textOptionsContainer.transform.GetChild(0).GetComponent<TextOptionUI>();
        topOption.OptionText.color = topOption.FadedFontColor;
        topOption.OptionText.text = "Continue";

        bodyScrollViewTransform.offsetMin = new Vector2(textContainerLeft, textContainerBottom);
        bodyScrollViewTransform.offsetMax = new Vector2(textContainerRight, textContainerTop);

        ContinueDialogueButton.interactable = true; // Re-enable continue button.
    }
}
