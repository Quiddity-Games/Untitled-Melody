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
/// The script that handles all of the branching dialogue / "Texting scene" functionality using the Inkle plugin. Attached to the Dialogue UI Canvas gameObject.
/// </summary>
[Serializable] 
public class Inkle : MonoBehaviour
{
    [FormerlySerializedAs("inkText")]
    [SerializeField] TextAsset inkTextAsset;

    [SerializeField] Button startDialogueButton;
    [SerializeField] Button continueDialogueButton;

    [Header("Phone UI: Overall")]
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] CanvasGroup cellPhoneCanvasGroup;
    [SerializeField] float canvasFadeDuration;
    [SerializeField] float startDialogueDelayDuration;
    [Header("Phone UI: Header")]
    [SerializeField] CanvasGroup headerCanvasGroup;
    [SerializeField] Image headerIcon;
    [SerializeField] TextMeshProUGUI headerText;
    [Header("Phone UI: Body")]
    [SerializeField] RectTransform bodyScrollViewTransform;
    [SerializeField] GameObject bodyScrollContent;
    [SerializeField] ScrollRect bodyScrollRect;

    [Space(10)]
    [Header("Bubbles")]
    [SerializeField] GameObject textBubblePrefab;
    [SerializeField] GameObject optionButtonPrefab;
    [SerializeField] GameObject textOptionsContainer;
    [SerializeField] float bubbleFadeDuration;

    #region Hidden Variables
    Dictionary<string, string> GlobalTagsDictionary = new Dictionary<string, string>();
    Dictionary<string, TextBubbleUIElements> CharacterUIDictionary = new Dictionary<string, TextBubbleUIElements>();

    [HideInInspector] float textContainerTop;
    [HideInInspector] float textContainerRight;
    [HideInInspector] float textContainerLeft;
    [HideInInspector] float textContainerBottom;

    [HideInInspector] int currentBubbleIndex;
    [HideInInspector] List<string> linesBeforeChoice = new List<string>();
    [HideInInspector] List<TextBubbleUI> bubblesBeforeChoice = new List<TextBubbleUI>();
    [HideInInspector] List<TextOptionUI> currentOptions = new List<TextOptionUI>();

    [HideInInspector] Story inkStory;
    [HideInInspector] TextBubbleCharacterUI characterUI;
    #endregion

    void Start()
    {
        // Get components and variables.
        inkStory = new Story(inkTextAsset.text);
        characterUI = GetComponent<TextBubbleCharacterUI>();

        // Set up UI layout and visibility.
        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.gameObject.transform as RectTransform);
        cellPhoneCanvasGroup.alpha = 0f;
        headerCanvasGroup.alpha = 0f;

        // Give functions to buttons.
        startDialogueButton.onClick.AddListener(ShowDialogueUI);
        continueDialogueButton.onClick.AddListener(PlayDialogue);

        GetDictionaryValues();
        linesBeforeChoice = GetLinesBeforeChoice();

        // Get current size of texting body container.
        textContainerLeft = bodyScrollViewTransform.offsetMin.x;
        textContainerRight = bodyScrollViewTransform.offsetMax.x;
        textContainerTop = bodyScrollViewTransform.offsetMax.y;
        textContainerBottom = bodyScrollViewTransform.offsetMin.y;
    }

    void GetDictionaryValues()
    {
        // Get global tags.
        for (int i = 0; i < inkStory.globalTags.Count; i++)
        {
            if (inkStory.globalTags[i].Contains("Conversation: "))
            {
                GlobalTagsDictionary.Add("Conversation", inkStory.globalTags[i].Replace("Conversation: ", ""));
            }
            else if (inkStory.globalTags[i].Contains("Alias: "))
            {
                GlobalTagsDictionary.Add("Alias", inkStory.globalTags[i].Replace("Alias: ", ""));
            }
        }

        // Get character UI elements.
        for (int i = 0; i < characterUI.CharacterUIElements.Count; i++)
        {
            CharacterUIDictionary.Add(characterUI.CharacterUIElements[i].CharacterName, characterUI.CharacterUIElements[i]);
        }
    }

    /// <summary>
    /// Show dialogue UI after delay on start.
    /// </summary>
    void ShowDialogueUI()
    {
        FadeInUI(cellPhoneCanvasGroup, canvasFadeDuration);
        GetHeaderText();
        startDialogueButton.gameObject.SetActive(false);
        continueDialogueButton.interactable = false;

        StartCoroutine(PlayFirstLine(canvasFadeDuration));

        IEnumerator PlayFirstLine(float duration)
        {
            float time = 0;
            duration += startDialogueDelayDuration;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            PlayDialogue();
            continueDialogueButton.interactable = true;
        }
    }

    /// <summary>
    /// Create a text bubble using the parsed text.
    /// </summary>
    /// <param name="line"></param>
    void CreateTextBubble(string line)
    {
        GameObject textBubble = Instantiate(textBubblePrefab, bodyScrollContent.transform);
        textBubble.SetActive(false);
        bubblesBeforeChoice.Add(textBubble.GetComponent<TextBubbleUI>());
        
        TextBubbleUI ui = textBubble.GetComponent<TextBubbleUI>();
        FadeInUI(ui.CanvasGroup, bubbleFadeDuration);

        string speakerName = ui.ParseSpeaker(line, GlobalTagsDictionary);

        ui.SetTextBubbleInformation(line, speakerName, characterUI.MainCharacterName, GetSenderIcon(speakerName));
    }

    /// <summary>
    /// Set the header text and icon.
    /// Can use an alias with tag "#Alias: x", if applicable.
    /// </summary>
    void GetHeaderText()
    {
        headerIcon.sprite = GetSenderIcon(GlobalTagsDictionary["Conversation"]);
        if (GlobalTagsDictionary.ContainsKey("Alias") && !string.IsNullOrEmpty(GlobalTagsDictionary["Alias"]))
            headerText.text = GlobalTagsDictionary["Alias"];
        else
            headerText.text = GlobalTagsDictionary["Conversation"];

        LayoutRebuilder.ForceRebuildLayoutImmediate(headerCanvasGroup.gameObject.transform as RectTransform);
        FadeInUI(headerCanvasGroup, bubbleFadeDuration);
    }

    /// <summary>
    /// Get a list of strings as all lines before choices, including tags.
    /// </summary>
    /// <returns></returns>
    List<string> GetLinesBeforeChoice()
    {
        List<string> lines = new List<string>();
        while (inkStory.canContinue)
        {
            string currentTextChunk = inkStory.Continue();
            List<string> currentTags = inkStory.currentTags;

            string line = "";

            foreach (string tag in currentTags)
            {
                line += ("#" + tag + "\n");
            }
            line += currentTextChunk;

            if (!string.IsNullOrEmpty(line))
            {
                lines.Add(line);
                CreateTextBubble(line);
            }
        }

        return lines;
    }

    /// <summary>
    /// Displays all current instantiated bubbles in order until dialogue ends or choices appear.
    /// </summary>
    void PlayDialogue()
    {
        if (bubblesBeforeChoice.Count > 0 && !bubblesBeforeChoice[bubblesBeforeChoice.Count - 1].gameObject.activeInHierarchy)
        {
            continueDialogueButton.interactable = true;

            bubblesBeforeChoice[currentBubbleIndex].gameObject.SetActive(true);
            FadeInUI(bubblesBeforeChoice[currentBubbleIndex].CanvasGroup, bubbleFadeDuration);
            bodyScrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom of the container.

            currentBubbleIndex++;
        }
        else
        {
            linesBeforeChoice.Clear();
            bubblesBeforeChoice.Clear();
            currentBubbleIndex = 0;
            continueDialogueButton.interactable = false;

            if (inkStory.currentChoices.Count > 0)
                DisplayOptions();
        }
    }

    /// <summary>
    /// Creates a container for options and instantiates a number of option buttons based on Ink.
    /// </summary>
    void DisplayOptions()
    {
        // Use the empty choice in container.
        TextOptionUI topOption = textOptionsContainer.transform.GetChild(0).GetComponent<TextOptionUI>();
        currentOptions.Add(topOption);
        topOption.OptionText.text = inkStory.currentChoices[0].text;
        topOption.OptionIndex = 0;

        for (int i = 1; i < inkStory.currentChoices.Count; i++)
        {
            GameObject optionButton = Instantiate(optionButtonPrefab, textOptionsContainer.transform);
            TextOptionUI ui = optionButton.GetComponent<TextOptionUI>();
            currentOptions.Add(ui);

            ui.OptionText.text = inkStory.currentChoices[i].text;
            ui.OptionIndex = i;
        }

        // Set event to buttons and fade them in gradually.
        foreach(TextOptionUI options in currentOptions)
        {
            options.OptionButton.onClick.AddListener(() => ChoiceMadeCallback(options.OptionIndex));
            FadeInUI(options.GetComponent<CanvasGroup>(), bubbleFadeDuration + options.OptionIndex * 0.15f);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.transform as RectTransform);

        bodyScrollViewTransform.offsetMin = new Vector2(textContainerLeft, (textOptionsContainer.transform as RectTransform).sizeDelta.y);
        bodyScrollViewTransform.offsetMax = new Vector2(textContainerRight, (headerCanvasGroup.gameObject.transform as RectTransform).sizeDelta.y);

    }

    /// <summary>
    /// Gets the sender's icon sprite from <see cref="textMessageElements"/> if name matches in-line Ink tag.
    /// </summary>
    /// <param name="senderName"></param>
    /// <returns></returns>
    Sprite GetSenderIcon(string senderName)
    {
        Sprite sprite = null;

        if (CharacterUIDictionary.ContainsKey(senderName))
        {
            sprite = CharacterUIDictionary[senderName].IconSprite;
        }

        return sprite;
    }

    /// <summary>
    /// Used when a choice is selected.
    /// Clears all current options objects before grabbing the next set of text objects.
    /// </summary>
    /// <param name="choice"></param>
    void ChoiceMadeCallback(int choice)
    {
        if (inkStory.currentChoices.Count > 0)
        {
            // Select route and destroy buttons.
            inkStory.ChooseChoiceIndex(choice);

            for (int i = 1; i < currentOptions.Count; i++)
            {
                Destroy(currentOptions[i].gameObject);
            }

            currentOptions.Clear();
            linesBeforeChoice = GetLinesBeforeChoice();
            PlayDialogue();

            // Reset container size values.
            TextOptionUI topOption = textOptionsContainer.transform.GetChild(0).GetComponent<TextOptionUI>();
            topOption.OptionText.text = "";

            bodyScrollViewTransform.offsetMin = new Vector2(textContainerLeft, textContainerBottom);
            bodyScrollViewTransform.offsetMax = new Vector2(textContainerRight, textContainerTop);

            continueDialogueButton.interactable = true; // Re-enable continue button.
        }
    }

    /// <summary>
    /// Used to fade in UI with a CanvasGroup component attached.
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="duration"></param>
    void FadeInUI(CanvasGroup canvasGroup, float duration)
    {
        StartCoroutine(FadeIn(duration));

        IEnumerator FadeIn(float duration)
        {
            float time = 0;

            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }
    }
}
