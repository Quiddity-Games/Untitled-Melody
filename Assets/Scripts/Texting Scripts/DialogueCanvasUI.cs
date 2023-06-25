using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Handles the entirety of the display of the texting UI, using references from the scene's <see cref="DialogueController"/>.
/// Attached to the Dialogue UI Canvas gameObject.
/// </summary>

[Serializable]
public class DialogueCanvasUI : MonoBehaviour
{
    #region Variables: Buttons
    [Header("Buttons")]
    [SerializeField] Button startDialogueButton;
    public Button ContinueDialogueButton;
    [SerializeField] Toggle autoplayToggleButton;
    [SerializeField] Button skipToChoiceButton;
    #endregion

    #region Variables: Cellphone UI
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
    public GameObject TextTypingContainer;
    #endregion

    #region Variables: Autoplay/Skip
    [Header("Phone UI: Autoplay/Skip")]
    [SerializeField] GameObject autoplaySkipContainer;
    public TextMeshProUGUI autoplayText;
    private Animator autoplaySkipAnimator;
    #endregion

    #region Hidden Variables
    [HideInInspector] float textContainerTop;
    [HideInInspector] float textContainerRight;
    [HideInInspector] float textContainerLeft;
    [HideInInspector] float textContainerBottom;

    private float canvasFadeDuration;
    private float startDelayDuration;
    private float bubbleFadeDuration;
    private float currentTypingDelayDuration;
    private float autoplayDelayDuration;

    // Typing bubble delays
    private float longTypingDelayDuration;
    private float midTypingDelayDuration;
    private float shortTypingDelayDuration;

    // Character info
    private Dictionary<string, TextBubbleUIElements> characterUIDictionary;
    private string mainCharacterName;

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
        autoplayToggleButton.onValueChanged.AddListener(delegate { DialogueController.Instance.SetAutoplay(); });
        skipToChoiceButton.onClick.AddListener(SkipToChoice);

        // Get current size of texting body container.
        textContainerLeft = bodyScrollViewTransform.offsetMin.x;
        textContainerRight = bodyScrollViewTransform.offsetMax.x;
        textContainerTop = bodyScrollViewTransform.offsetMax.y;
        textContainerBottom = bodyScrollViewTransform.offsetMin.y;
    }

    /// <summary>
    /// Get all starting references from the <see cref="DialogueController"/>.
    /// Called from the Dialogue Controller after Awake to ensure correct order of operations.
    /// </summary>
    public void GetReferencesFromController()
    {
        autoplaySkipAnimator = autoplaySkipContainer.GetComponent<Animator>();

        #region Get values from DialogueController
        canvasFadeDuration = DialogueController.Instance.CanvasFadeDuration;
        startDelayDuration = DialogueController.Instance.StartDelayDuration;
        bubbleFadeDuration = DialogueController.Instance.BubbleFadeDuration;
        autoplayDelayDuration = DialogueController.Instance.AutoplayDelayDuration;
        characterUIDictionary = DialogueController.Instance.CharacterUIDictionary;
        longTypingDelayDuration = DialogueController.Instance.LongTypingDelayDuration;
        midTypingDelayDuration = DialogueController.Instance.MidTypingDelayDuration;
        shortTypingDelayDuration = DialogueController.Instance.ShortTypingDelayDuration;

        mainCharacterName = TextBubbleCharacterUI.Instance.MainCharacterName;
        #endregion
    }

    /// <summary>
    /// Show dialogue UI after delay on start.
    /// </summary>
    void ShowDialogueUI()
    {
        DialogueController.Instance.FadeInUI(phoneContainerCanvasGroup, canvasFadeDuration);
        GetHeaderText();
        startDialogueButton.gameObject.SetActive(false);
        ContinueDialogueButton.interactable = false;

        StartCoroutine(PlayFirstLine(canvasFadeDuration));

        IEnumerator PlayFirstLine(float duration)
        {
            float time = 0;
            duration += startDelayDuration;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            if (DialogueController.Instance.Autoplay)
                AutoplayDialogue();
            else
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
        DialogueController.Instance.FadeInUI(ui.CanvasGroup, bubbleFadeDuration);

        ui.SetTextBubbleInformation(line, mainCharacterName, characterUIDictionary[speakerName]);
    }

    /// <summary>
    /// Set the header text and icon.
    /// Can use an alias with tag "#Alias: x", if applicable (not currently in use).
    /// </summary>
    void GetHeaderText()
    {
        headerIcon.sprite = characterUIDictionary[DialogueController.Instance.GlobalTagsDictionary["Conversation"]].IconSprite;
        headerText.text = DialogueController.Instance.GlobalTagsDictionary["Conversation"];

        LayoutRebuilder.ForceRebuildLayoutImmediate(headerCanvasGroup.gameObject.transform as RectTransform);
        DialogueController.Instance.FadeInUI(headerCanvasGroup, bubbleFadeDuration);
    }

    /// <summary>
    /// Displays all current instantiated bubbles in order until dialogue ends or choices appear.
    /// </summary>
    public void PlayDialogue()
    {
        List<TextBubbleUI> bubblesBeforeChoice = DialogueController.Instance.BubblesBeforeChoice;
        int currentBubbleIndex = DialogueController.Instance.CurrentBubbleIndex;
        
        DialogueController.Instance.CanPrintDialogue = false;

        if (bubblesBeforeChoice.Count > 0 && !bubblesBeforeChoice[bubblesBeforeChoice.Count - 1].gameObject.activeInHierarchy)
        {
            // Get the correctly aligned typing bubble.
            CanvasGroup typingCanvasGroup = DialogueController.Instance.GetCurrentBubble(bubblesBeforeChoice[currentBubbleIndex].SenderNameText.text);

            StartCoroutine(ShowTextTypingBubble());

            IEnumerator ShowTextTypingBubble()
            {
                // Set the typing bubble to the bottom.
                TextTypingContainer.transform.SetAsLastSibling();

                if (!typingCanvasGroup.gameObject.activeInHierarchy)
                {
                    typingCanvasGroup.gameObject.SetActive(true);
                    DialogueController.Instance.FadeInUI(typingCanvasGroup, bubbleFadeDuration);
                    TMP_TextInfo info = bubblesBeforeChoice[currentBubbleIndex].MessageText.GetTextInfo(bubblesBeforeChoice[currentBubbleIndex].MessageText.text);

                    float duration = 0f;
                    float time = 0f;

                    // Change the duration of the typing bubble based on line length.
                    if (info.characterCount >= 100)
                        duration = longTypingDelayDuration;
                    else if (info.lineCount >= 50 && info.lineCount <= 100)
                        duration = midTypingDelayDuration;
                    else
                        duration = shortTypingDelayDuration;

                    currentTypingDelayDuration = duration;

                    while (time < duration)
                    {
                        time += Time.deltaTime;
                        yield return null;
                    }
                }

                if (bubblesBeforeChoice.Count > 0)
                {
                    typingCanvasGroup.gameObject.SetActive(false);
                    ShowNextTextBubble(bubblesBeforeChoice, currentBubbleIndex);
                }
            }
        }
        else if (bubblesBeforeChoice[bubblesBeforeChoice.Count - 1].gameObject.activeInHierarchy)
        {
            // If there are choices to be shown, show them.
            if (DialogueController.Instance.InkStory.currentChoices.Count > 0)
                DisplayChoices();
        }
    }

    /// <summary>
    /// Used to print the next text bubble after the typing bubble animation clears.
    /// </summary>
    /// <param name="bubblesBeforeChoice"></param>
    /// <param name="currentBubbleIndex"></param>
    void ShowNextTextBubble(List<TextBubbleUI> bubblesBeforeChoice, int currentBubbleIndex)
    {
        DialogueController.Instance.CurrentTypingBubble.gameObject.SetActive(false);

        ContinueDialogueButton.interactable = true;

        // Check the current index of the shown bubble, and set it to true.
        bubblesBeforeChoice[currentBubbleIndex].gameObject.SetActive(true);
        DialogueController.Instance.FadeInUI(bubblesBeforeChoice[currentBubbleIndex].CanvasGroup, bubbleFadeDuration);
        bodyScrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom of the container.

        DialogueController.Instance.CurrentBubbleIndex++;
        DialogueController.Instance.CanPrintDialogue = true;

    }

    /// <summary>
    /// Creates a container for options and instantiates a number of option buttons based on Ink.
    /// </summary>
    void DisplayChoices()
    {
        ContinueDialogueButton.interactable = false;

        // Reset all current text bubble values for DialogueController.
        DialogueController.Instance.LinesBeforeChoice.Clear();
        DialogueController.Instance.BubblesBeforeChoice.Clear();
        DialogueController.Instance.CurrentBubbleIndex = 0;

        List<TextOptionUI> currentOptions = DialogueController.Instance.CurrentOptions;
        Story inkStory = DialogueController.Instance.InkStory;

        // Show a typing bubble animation.
        TextTypingContainer.transform.SetAsLastSibling();
        CanvasGroup typingCanvasGroup = DialogueController.Instance.RightTypingBubble;
        typingCanvasGroup.gameObject.SetActive(true);
        DialogueController.Instance.FadeInUI(typingCanvasGroup, bubbleFadeDuration);

        ContinueDialogueButton.gameObject.SetActive(false);
        
        // Create a button for each available choice.
        for (int i = 0; i < inkStory.currentChoices.Count; i++)
        {
            GameObject optionButton = Instantiate(DialogueController.Instance.OptionButtonPrefab, textOptionsContainer.transform);
            TextOptionUI ui = optionButton.GetComponent<TextOptionUI>();
            ui.OptionFontColour = characterUIDictionary[mainCharacterName].FontColor;

            currentOptions.Add(ui);

            ui.OptionText.color = ui.OptionFontColour;
            ui.OptionText.text = inkStory.currentChoices[i].text;
            ui.OptionIndex = i;
        }

        // Set event to buttons and fade them in gradually.
        foreach(TextOptionUI options in currentOptions)
        {
            options.OptionButton.onClick.AddListener(() => DialogueController.Instance.ChoiceMadeCallback(options.OptionIndex));
            DialogueController.Instance.FadeInUI(options.GetComponent<CanvasGroup>(), bubbleFadeDuration + options.OptionIndex * 0.15f);
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

    /// <summary>
    /// Used to skip directly to the next choice. Shows all bubbles and then <see cref="DisplayChoices"/>.
    /// </summary>
    public void SkipToChoice()
    {
        List<TextBubbleUI> bubblesBeforeChoice = DialogueController.Instance.BubblesBeforeChoice;
        Story inkStory = DialogueController.Instance.InkStory;
        DisplayAutoplaySkipOptions(false);

        if (bubblesBeforeChoice.Count > 0)
        {
            for (int i = 0; i < bubblesBeforeChoice.Count; i++)
            {
                if (!bubblesBeforeChoice[i].gameObject.activeInHierarchy)
                {
                    bubblesBeforeChoice[i].gameObject.SetActive(true);
                    DialogueController.Instance.FadeInUI(bubblesBeforeChoice[i].CanvasGroup, bubbleFadeDuration);
                }
            }
        }

        if (inkStory.currentChoices.Count > 0)
            DisplayChoices();
    }

    /// <summary>
    /// Used to autoplay dialogue if the setting is on.
    /// Only prints after a bubble has finished printing.
    /// </summary>
    public void AutoplayDialogue()
    {
        List<TextBubbleUI> bubblesBeforeChoice = DialogueController.Instance.BubblesBeforeChoice;
        int currentBubbleIndex = DialogueController.Instance.CurrentBubbleIndex;
        bool autoplay = DialogueController.Instance.Autoplay;

        if (currentBubbleIndex < bubblesBeforeChoice.Count - 1)
            StartCoroutine(Autoplay());

        IEnumerator Autoplay()
        {
            while (currentBubbleIndex < bubblesBeforeChoice.Count - 1)
            {
                while (!DialogueController.Instance.CanPrintDialogue)
                {
                    yield return null;
                }

                if (autoplay)
                {
                    PlayDialogue();
                    yield return new WaitForSeconds(autoplayDelayDuration + currentTypingDelayDuration);
                }
                yield return null;
            }
        }
    }

    /// <summary>
    /// Used to animate the autoplay and skip to choice options menu.
    /// </summary>
    /// <param name="on"></param>
    public void DisplayAutoplaySkipOptions(bool on)
    {
        if (on)
        {
            Time.timeScale = 0f;
            autoplaySkipAnimator.SetTrigger("Open");
        } else
        {
            Time.timeScale = 1f;
            autoplaySkipAnimator.SetTrigger("Close");

            if (DialogueController.Instance.Autoplay)
                AutoplayDialogue();
        }
    }
}
